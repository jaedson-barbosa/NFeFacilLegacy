using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using BaseGeral.ModeloXML;
using BaseGeral.Controles;
using BaseGeral.ItensBD;
using System.Linq;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesIdentificacao;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTotal;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using BaseGeral.IBGE;
using BaseGeral.Validacao;
using Windows.UI.Xaml;
using Comum.CaixasDialogo;
using NFeFacil.View;
using BaseGeral;
using BaseGeral.View;
using Fiscal;
using BaseGeral.Buscador;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum
{
    [DetalhePagina(Symbol.Document, "Nota fiscal")]
    public sealed partial class ManipulacaoNotaFiscal : Page, IHambuguer
    {
        public ManipulacaoNotaFiscal()
        {
            InitializeComponent();
            Clientes = new BuscadorCliente();
            Motoristas = new BuscadorMotoristaComVeiculos();
        }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Tag, "Identificação"),
            new ItemHambuguer(Symbol.People, "Cliente"),
            new ItemHambuguer(Symbol.Street, "Retirada/Entrega"),
            new ItemHambuguer(Symbol.Calculator, "Totais"),
            new ItemHambuguer("\uE806", "Transporte"),
            new ItemHambuguer("\uE825", "Cobrança"),
            new ItemHambuguer(Symbol.Comment, "Informações adicionais"),
            new ItemHambuguer(Symbol.World, "Exportação e compras"),
            new ItemHambuguer("\uEC0A", "Cana-de-açúcar"),
            new ItemHambuguer(Symbol.More, "Pagamento")
        };

        public int SelectedIndex { set => main.SelectedIndex = value; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var Dados = (NFe)e.Parameter;

            if (Dados.Informacoes.total == null)
                Dados.Informacoes.total = new Total(Dados.Informacoes.produtos);
            NotaSalva = Dados;

            MunicipiosIdentificacao = new ObservableCollection<Municipio>(Municipios.Get(NotaSalva.Informacoes.identificacao.CódigoUF));
            MunicipiosTransporte = new ObservableCollection<Municipio>(Municipios.Get(UFEscolhida));
            NFesReferenciadas = new ObservableCollection<DocumentoFiscalReferenciado>(NotaSalva.Informacoes.identificacao.DocumentosReferenciados.Where(x => !string.IsNullOrEmpty(x.RefNFe)));
            NFsReferenciadas = new ObservableCollection<DocumentoFiscalReferenciado>(NotaSalva.Informacoes.identificacao.DocumentosReferenciados.Where(x => x.RefNF != null));
            Reboques = new ObservableCollection<Reboque>(NotaSalva.Informacoes.transp.Reboque);
            Volumes = new ObservableCollection<Volume>(NotaSalva.Informacoes.transp.Vol);
            Duplicatas = new ObservableCollection<Duplicata>(NotaSalva.Informacoes.cobr.Dup);
            FornecimentosDiarios = new ObservableCollection<FornecimentoDiario>(NotaSalva.Informacoes.cana.ForDia);
            Deducoes = new ObservableCollection<Deducoes>(NotaSalva.Informacoes.cana.Deduc);
            Observacoes = new ObservableCollection<Observacao>(NotaSalva.Informacoes.infAdic.ObsCont);
            ProcessosReferenciados = new ObservableCollection<ProcessoReferenciado>(NotaSalva.Informacoes.infAdic.ProcRef);
            FormasPagamento = NotaSalva.Informacoes.FormasPagamento.Select(x => new FormaPagamento(x)).GerarObs();

            DataPrestacao = string.IsNullOrEmpty(NotaSalva.Informacoes.total.ISSQNtot?.DCompet)
                ? DateTimeOffset.Now
                : DateTimeOffset.Parse(NotaSalva.Informacoes.total.ISSQNtot.DCompet);
            CRegTrib = (NotaSalva.Informacoes.total.ISSQNtot?.CRegTrib - 1) ?? 0;
            RetTrib = NotaSalva.Informacoes.total.RetTrib ?? new RetTrib();

            AtualizarVeiculo();
            NotaSalva.Informacoes.total = new Total(NotaSalva.Informacoes.produtos);
        }

        const string NomeClienteHomologacao = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

        NFe NotaSalva { get; set; }

        #region Dados base

        BuscadorCliente Clientes { get; }
        BuscadorMotoristaComVeiculos Motoristas { get; }

        private ClienteDI clienteSelecionado;
        public ClienteDI ClienteSelecionado
        {
            get
            {
                var dest = NotaSalva.Informacoes.destinatario;
                if (clienteSelecionado == null && dest != null)
                {
                    clienteSelecionado = Clientes.BuscarViaDocumento(dest.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                clienteSelecionado = value;
                NotaSalva.Informacoes.destinatario = value?.ToDestinatario();
                if (NotaSalva.AmbienteTestes && NotaSalva.Informacoes.destinatario != null)
                {
                    NotaSalva.Informacoes.destinatario.Nome = NomeClienteHomologacao;
                }
            }
        }

        private MotoristaManipulacaoNFe motoristaSelecionado;
        MotoristaManipulacaoNFe MotoristaSelecionado
        {
            get
            {
                var mot = NotaSalva.Informacoes.transp?.Transporta;
                if (motoristaSelecionado.Equals(default(MotoristaManipulacaoNFe)) && mot?.Documento != null)
                {
                    motoristaSelecionado = Motoristas.BuscarViaDocumento(mot.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                motoristaSelecionado = value;
                NotaSalva.Informacoes.transp.Transporta = value.Root?.ToMotorista();
                ProcessarVeiculo(value);
            }
        }

        async void ProcessarVeiculo(MotoristaManipulacaoNFe mot)
        {
            VeiculoDI escolhido = null;
            if (mot.Secundarios?.Length > 0)
            {
                var caixa = new EscolherVeiculo(mot.Secundarios, mot.Principal);
                await caixa.ShowAsync();
                escolhido = caixa.Escolhido;
            }
            else if (mot.Principal != null)
            {
                escolhido = mot.Principal;
            }

            NotaSalva.Informacoes.transp.VeicTransp = escolhido != null ? escolhido.ToVeiculo() : new Veiculo();
            AtualizarVeiculo();
        }

        void AtualizarVeiculo()
        {
            ctrPaiVeiculo.DataContext = NotaSalva.Informacoes.transp.VeicTransp;
        }

        #endregion

        #region Identificação

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(NotaSalva.Informacoes.identificacao.DataHoraEmissão))
                {
                    var agora = DefinicoesTemporarias.DateTimeNow;
                    NotaSalva.Informacoes.identificacao.DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(NotaSalva.Informacoes.identificacao.DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informacoes.identificacao.DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                NotaSalva.Informacoes.identificacao.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informacoes.identificacao.DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                NotaSalva.Informacoes.identificacao.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (string.IsNullOrEmpty(NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada))
                {
                    var agora = DefinicoesTemporarias.DateTimeNow;
                    NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraSaidaEntrada
        {
            get => DataSaidaEntrada.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                NotaSalva.Informacoes.identificacao.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public int CodigoMunicipio
        {
            get => NotaSalva.Informacoes.identificacao.CodigoMunicipio;
            set => NotaSalva.Informacoes.identificacao.CodigoMunicipio = value;
        }

        public int FinalidadeEmissao
        {
            get => NotaSalva.Informacoes.identificacao.FinalidadeEmissao - 1;
            set => NotaSalva.Informacoes.identificacao.FinalidadeEmissao = value + 1;
        }

        public int IdentificadorDestino
        {
            get => NotaSalva.Informacoes.identificacao.IdentificadorDestino - 1;
            set => NotaSalva.Informacoes.identificacao.IdentificadorDestino = value + 1;
        }

        #endregion

        #region Transporte

        private Estado ufEscolhida;
        public Estado UFEscolhida
        {
            get
            {
                if (NotaSalva.Informacoes.transp.RetTransp.CMunFG != 0 && ufEscolhida == null)
                {
                    foreach (var item in Estados.EstadosCache)
                    {
                        var lista = Municipios.Get(item);
                        if (lista.Count(x => x.Codigo == NotaSalva.Informacoes.transp.RetTransp.CMunFG) > 0)
                        {
                            ufEscolhida = item;
                            break;
                        }
                    }
                }
                return ufEscolhida;
            }
            set
            {
                ufEscolhida = value;
                MunicipiosTransporte.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    MunicipiosTransporte.Add(item);
                }
            }
        }

        public string ModFrete
        {
            get => NotaSalva.Informacoes.transp.ModFrete.ToString();
            set => NotaSalva.Informacoes.transp.ModFrete = ushort.Parse(value);
        }

        public double CFOP
        {
            get => NotaSalva.Informacoes.transp.RetTransp.CFOP;
            set => NotaSalva.Informacoes.transp.RetTransp.CFOP = (long)value;
        }

        #endregion

        #region Totais

        DateTimeOffset DataPrestacao { get; set; }
        int CRegTrib { get; set; }
        RetTrib RetTrib { get; set; }

        #endregion

        void Confirmar()
        {
            try
            {
                if (NotaSalva.AmbienteTestes)
                {
                    NotaSalva.Informacoes.destinatario.Nome = NomeClienteHomologacao;
                }

                var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                if (ultPage.SourcePageType.Name == "VisualizacaoRegistroVenda")
                {
                    Frame.BackStack.Remove(ultPage);
                    ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                }
                Frame.BackStack.Remove(ultPage);
                ultPage = Frame.BackStack[Frame.BackStack.Count - 1];

                NotaSalva.Informacoes.total.ISSQNtot.DCompet = DataPrestacao.ToString("yyyy-MM-dd");
                NotaSalva.Informacoes.total.ISSQNtot.CRegTrib = CRegTrib + 1;
                NotaSalva.Informacoes.total.RetTrib = RetTrib;

                var nota = NotaSalva;
                new AnalisadorNFe(ref nota).Normalizar();

                using (var repo = new BaseGeral.Repositorio.OperacoesExtras())
                {
                    string IDOriginal = nota.Informacoes.Id;
                    nota.Informacoes.AtualizarChave();
                    string NovoId = nota.Informacoes.Id;

                    repo.ProcessarNFeLocal(IDOriginal, NovoId);
                }

                if (ultPage.SourcePageType != typeof(Visualizacao))
                {
                    var novoDI = new NFeDI(nota, nota.ToXElement().ToString())
                    {
                        Status = (int)StatusNota.Validada
                    };
                    var acoes = new AcoesNFe(novoDI, nota);
                    Frame.BackStack.Add(new PageStackEntry(typeof(Visualizacao), acoes, null));
                }
                else
                {
                    var acoes = (AcoesNFe)ultPage.Parameter;
                    var di = acoes.ItemBanco;
                    di.Id = nota.Informacoes.Id;
                    di.NomeCliente = nota.Informacoes.destinatario.Nome;
                    di.DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
                    di.Status = (int)StatusNota.Validada;
                    di.XML = nota.ToXElement().ToString();
                }

                BasicMainPage.Current.Retornar();
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        #region ColecoesExibicaoView

        ObservableCollection<Municipio> MunicipiosIdentificacao { get; set; }
        public ObservableCollection<Municipio> MunicipiosTransporte { get; set; }
        ObservableCollection<DocumentoFiscalReferenciado> NFesReferenciadas { get; set; }
        ObservableCollection<DocumentoFiscalReferenciado> NFsReferenciadas { get; set; }
        ObservableCollection<Reboque> Reboques { get; set; }
        ObservableCollection<Volume> Volumes { get; set; }
        ObservableCollection<Duplicata> Duplicatas { get; set; }
        ObservableCollection<FornecimentoDiario> FornecimentosDiarios { get; set; }
        ObservableCollection<Deducoes> Deducoes { get; set; }
        ObservableCollection<Observacao> Observacoes { get; set; }
        ObservableCollection<ProcessoReferenciado> ProcessosReferenciados { get; set; }
        ObservableCollection<FormaPagamento> FormasPagamento { get; set; }

        #endregion

        #region Adição e remoção básica

        async void AdicionarNFeReferenciada()
        {
            var caixa = new AdicionarReferenciaEletronica();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = new DocumentoFiscalReferenciado
                {
                    RefNFe = caixa.Chave
                };
                NotaSalva.Informacoes.identificacao.DocumentosReferenciados.Add(novo);
                NFesReferenciadas.Add(novo);
            }
        }

        async void AdicionarNFReferenciada()
        {
            var caixa = new AdicionarNF1AReferenciada();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = new DocumentoFiscalReferenciado
                {
                    RefNF = caixa.Contexto
                };
                NotaSalva.Informacoes.identificacao.DocumentosReferenciados.Add(novo);
                NFsReferenciadas.Add(novo);
            }
        }

        void RemoverDocReferenciado(DocumentoFiscalReferenciado doc)
        {
            NotaSalva.Informacoes.identificacao.DocumentosReferenciados.Remove(doc);
            if (string.IsNullOrEmpty(doc.RefNFe))
            {
                NFsReferenciadas.Remove(doc);
            }
            else
            {
                NFesReferenciadas.Remove(doc);
            }
        }

        async void AdicionarReboque()
        {
            var add = new AdicionarReboque();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = add.Contexto;
                NotaSalva.Informacoes.transp.Reboque.Add(novo);
                Reboques.Add(novo);
            }
        }

        void RemoverReboque(Reboque reboque)
        {
            NotaSalva.Informacoes.transp.Reboque.Remove(reboque);
            Reboques.Remove(reboque);
        }

        async void AdicionarVolume()
        {
            var add = new AdicionarVolume();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = add.Contexto;
                NotaSalva.Informacoes.transp.Vol.Add(novo);
                Volumes.Add(novo);
            }
        }

        void RemoverVolume(Volume volume)
        {
            NotaSalva.Informacoes.transp.Vol.Remove(volume);
            Volumes.Remove(volume);
        }

        async void AdicionarDuplicata()
        {
            var caixa = new AdicionarDuplicata();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.Contexto;
                NotaSalva.Informacoes.cobr.Dup.Add(novo);
                Duplicatas.Add(novo);
            }
        }

        void RemoverDuplicata(Duplicata duplicata)
        {
            NotaSalva.Informacoes.cobr.Dup.Remove(duplicata);
            Duplicatas.Remove(duplicata);
        }

        async void AdicionarFornecimento()
        {
            var caixa = new AdicionarFornecimentoDiario();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.Contexto;
                NotaSalva.Informacoes.cana.ForDia.Add(novo);
                FornecimentosDiarios.Add(novo);
            }
        }

        void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            NotaSalva.Informacoes.cana.ForDia.Remove(fornecimento);
            FornecimentosDiarios.Remove(fornecimento);
        }

        async void AdicionarDeducao()
        {
            var caixa = new AdicionarDeducao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.Contexto;
                NotaSalva.Informacoes.cana.Deduc.Add(novo);
                Deducoes.Add(novo);
            }
        }

        void RemoverDeducao(Deducoes deducao)
        {
            NotaSalva.Informacoes.cana.Deduc.Remove(deducao);
            Deducoes.Remove(deducao);
        }

        async void AdicionarObsContribuinte()
        {
            var caixa = new AdicionarObservacaoContribuinte();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.Contexto;
                NotaSalva.Informacoes.infAdic.ObsCont.Add(novo);
                Observacoes.Add(novo);
            }
        }

        void RemoverObsContribuinte(Observacao obs)
        {
            NotaSalva.Informacoes.infAdic.ObsCont.Remove(obs);
            Observacoes.Remove(obs);
        }

        async void AdicionarProcReferenciado()
        {
            var caixa = new AdicionarProcessoReferenciado();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.Item;
                NotaSalva.Informacoes.infAdic.ProcRef.Add(novo);
                ProcessosReferenciados.Add(novo);
            }
        }

        void RemoverProcReferenciado(ProcessoReferenciado proc)
        {
            NotaSalva.Informacoes.infAdic.ProcRef.Remove(proc);
            ProcessosReferenciados.Remove(proc);
        }

        #endregion

        #region Métodos View - Backcode

        private void AdicionarNFeReferenciada(object sender, RoutedEventArgs e)
        {
            AdicionarNFeReferenciada();
        }

        private void RemoverDocReferenciado(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverDocReferenciado((DocumentoFiscalReferenciado)contexto);
        }

        private void AdicionarNFReferenciada(object sender, RoutedEventArgs e)
        {
            AdicionarNFReferenciada();
        }

        private void AdicionarReboque(object sender, RoutedEventArgs e)
        {
            AdicionarReboque();
        }

        private void RemoverReboque(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverReboque((Reboque)contexto);
        }

        private void AdicionarVolume(object sender, RoutedEventArgs e)
        {
            AdicionarVolume();
        }

        private void RemoverVolume(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverVolume((Volume)contexto);
        }

        private void AdicionarDuplicata(object sender, RoutedEventArgs e)
        {
            AdicionarDuplicata();
        }

        private void RemoverDuplicata(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverDuplicata((Duplicata)contexto);
        }

        private void AdicionarObsContribuinte(object sender, RoutedEventArgs e)
        {
            AdicionarObsContribuinte();
        }

        private void RemoverObsContribuinte(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverObsContribuinte((Observacao)contexto);
        }

        private void AdicionarProcReferenciado(object sender, RoutedEventArgs e)
        {
            AdicionarProcReferenciado();
        }

        private void RemoverProcReferenciado(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverProcReferenciado((ProcessoReferenciado)contexto);
        }

        private void AdicionarFornecimento(object sender, RoutedEventArgs e)
        {
            AdicionarFornecimento();
        }

        private void RemoverFornecimento(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverFornecimento((FornecimentoDiario)contexto);
        }

        private void AdicionarDeducao(object sender, RoutedEventArgs e)
        {
            AdicionarDeducao();
        }

        private void RemoverDeducao(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverDeducao((Deducoes)contexto);
        }

        private void Confirmar(object sender, RoutedEventArgs e)
        {
            Confirmar();
        }

        #endregion

        private async void AlterarEnderecoRetirada(object sender, RoutedEventArgs e)
        {
            var controle = (ToggleSwitch)sender;
            if (controle.IsOn)
            {
                var caixa = new EscolherTipoEndereco();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    var tipo = caixa.TipoEscolhido;
                    if (!caixa.Nacional)
                    {
                        var caixa2 = new EnderecoDiferenteExterior();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Retirada = caixa2.Endereco;
                        }
                    }
                    else
                    {
                        var caixa2 = new EnderecoDiferenteNacional();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Retirada = caixa2.Endereco;
                        }
                    }
                }
            }
            else
            {
                NotaSalva.Informacoes.Retirada = null;
            }

            if (controle.IsOn && NotaSalva.Informacoes.Retirada == null)
            {
                controle.IsOn = false;
            }
        }

        private async void AlterarEnderecoEntrega(object sender, RoutedEventArgs e)
        {
            var controle = (ToggleSwitch)sender;
            if (controle.IsOn)
            {
                var caixa = new EscolherTipoEndereco();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    var tipo = caixa.TipoEscolhido;
                    if (!caixa.Nacional)
                    {
                        var caixa2 = new EnderecoDiferenteExterior();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Entrega = caixa2.Endereco;
                        }
                    }
                    else
                    {
                        var caixa2 = new EnderecoDiferenteNacional();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Entrega = caixa2.Endereco;
                        }
                    }
                }
            }
            else
            {
                NotaSalva.Informacoes.Entrega = null;
            }

            if (controle.IsOn && NotaSalva.Informacoes.Entrega == null)
            {
                controle.IsOn = false;
            }
        }

        private void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            var input = (GridView)sender;
            var cliente = input.SelectedItem;
            if (cliente != null)
            {
                input.ScrollIntoView(cliente, ScrollIntoViewAlignment.Leading);
            }
        }
        
        private void BuscarCliente(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Clientes.Buscar(busca);
        }

        private void BuscarMotorista(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Motoristas.Buscar(busca);
        }


        async void AdicionarFormaPagamento(object sender, RoutedEventArgs e)
        {
            var caixa = new AddFormaPagamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informacoes.FormasPagamento.Add(caixa.Pagamento);
                FormasPagamento.Add(new FormaPagamento(caixa.Pagamento));
            }
        }

        private void RemoverFormaPagamento(object sender, RoutedEventArgs e)
        {
            var forma = (FormaPagamento)((FrameworkElement)sender).DataContext;
            NotaSalva.Informacoes.FormasPagamento.Remove(forma.Original);
            FormasPagamento.Remove(forma);
        }

        void Voltar(object sender, RoutedEventArgs e)
        {
            var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
            var controle = (ControleViewProduto)ultPage.Parameter;
            controle.AtualizarControle(NotaSalva);
            controle.Voltar();
        }
    }
}
