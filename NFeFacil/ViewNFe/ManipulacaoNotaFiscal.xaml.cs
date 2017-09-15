using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Collections;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.View.Controles;
using NFeFacil.ItensBD;
using System.Collections.Generic;
using System.Linq;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao;
using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.Log;
using NFeFacil.Validacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoNotaFiscal : Page, IHambuguer, IValida
    {
        public ManipulacaoNotaFiscal()
        {
            InitializeComponent();
        }

        public IEnumerable ConteudoMenu
        {
            get
            {
                var retorno = new ObservableCollection<ItemHambuguer>
                {
                    new ItemHambuguer(Symbol.Tag, "Identificação"),
                    new ItemHambuguer(Symbol.People, "Cliente"),
                    new ItemHambuguer(Symbol.Street, "Retirada/Entrega"),
                    new ItemHambuguer(Symbol.Shop, "Produtos"),
                    new ItemHambuguer(Symbol.Calculator, "Totais"),
                    new ItemHambuguer("\uE806", "Transporte"),
                    new ItemHambuguer("\uE825", "Cobrança"),
                    new ItemHambuguer(Symbol.Comment, "Informações adicionais"),
                    new ItemHambuguer(Symbol.World, "Exportação e compras"),
                    new ItemHambuguer(new Uri("ms-appx:///Assets/CanaAcucar.png"), "Cana-de-açúcar")
                };
                pvtPrincipal.SelectionChanged += (sender, e) => MainMudou?.Invoke(this, new NewIndexEventArgs { NewIndex = pvtPrincipal.SelectedIndex });
                return retorno;
            }
        }

        public event EventHandler MainMudou;
        public void AtualizarMain(int index) => pvtPrincipal.SelectedIndex = index;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var Dados = (NFe)e.Parameter;
            if (string.IsNullOrEmpty(Dados.Informações.Id))
            {
                MainPage.Current.SeAtualizar(Symbol.Add, "Nota fiscal");
            }
            else
            {
                MainPage.Current.SeAtualizar(Symbol.Edit, "Nota fiscal");
            }

            using (var db = new AplicativoContext())
            {
                ClientesDisponiveis = db.Clientes.ToList();
                EmitentesDisponiveis = db.Emitentes.ToList();
                MotoristasDisponiveis = db.Motoristas.ToList();
                ProdutosDisponiveis = db.Produtos.ToList();
            }

            if (Dados.Informações.total == null)
                Dados.Informações.total = new Total(Dados.Informações.produtos);
            NotaSalva = Dados;

            Produtos = new ObservableCollection<DetalhesProdutos>(NotaSalva.Informações.produtos);

            Analisador = new AnalisadorNFe(NotaSalva);
            OperacoesNota = new OperacoesNotaSalva(Log);
        }

        const string NomeClienteHomologacao = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

        public NFe NotaSalva { get; private set; }

        #region Dados base
        public List<ClienteDI> ClientesDisponiveis { get; set; }
        public List<EmitenteDI> EmitentesDisponiveis { get; set; }
        public List<ProdutoDI> ProdutosDisponiveis { get; set; }
        public List<MotoristaDI> MotoristasDisponiveis { get; set; }

        private ClienteDI clienteSelecionado;
        public ClienteDI ClienteSelecionado
        {
            get
            {
                var dest = NotaSalva.Informações.destinatário;
                if (clienteSelecionado == null && dest != null)
                {
                    clienteSelecionado = ClientesDisponiveis.FirstOrDefault(x => x.Documento == dest.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                clienteSelecionado = value;
                NotaSalva.Informações.destinatário = value.ToDestinatario();
                if (NotaSalva.AmbienteTestes)
                {
                    NotaSalva.Informações.destinatário.Nome = NomeClienteHomologacao;
                }
            }
        }

        private EmitenteDI emitenteSelecionado;
        public EmitenteDI EmitenteSelecionado
        {
            get
            {
                var emit = NotaSalva.Informações.emitente;
                if (emitenteSelecionado == null && emit != null)
                {
                    emitenteSelecionado = EmitentesDisponiveis.FirstOrDefault(x => long.Parse(x.CNPJ) == emit.CNPJ);
                }
                return emitenteSelecionado;
            }
            set
            {
                emitenteSelecionado = value;
                NotaSalva.Informações.emitente = value.ToEmitente();
            }
        }

        public ProdutoDI ProdutoSelecionado { get; set; }

        private MotoristaDI motoristaSelecionado;
        public MotoristaDI MotoristaSelecionado
        {
            get
            {
                var mot = NotaSalva.Informações.transp?.Transporta;
                if (motoristaSelecionado == null && mot != null && mot.Documento != null)
                {
                    motoristaSelecionado = MotoristasDisponiveis.FirstOrDefault(x => x.Documento == mot.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                motoristaSelecionado = value;
                var transporte = NotaSalva.Informações.transp;
                transporte.Transporta = value.ToMotorista();
                if (value.Veiculo != default(Guid))
                {
                    using (var db = new AplicativoContext())
                        transporte.VeicTransp = db.Veiculos.Find(value.Veiculo).ToVeiculo();
                }
                else
                {
                    transporte.VeicTransp = new Veiculo();
                }
                AtualizarVeiculo();
            }
        }

        void AtualizarVeiculo()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Identificação

        public ObservableCollection<DocumentoFiscalReferenciado> NFesReferenciadas => NotaSalva.Informações.identificação.DocumentosReferenciados.Where(x => !string.IsNullOrEmpty(x.RefNFe)).GerarObs();
        public ObservableCollection<DocumentoFiscalReferenciado> NFsReferenciadas => NotaSalva.Informações.identificação.DocumentosReferenciados.Where(x => x.RefNF != null).GerarObs();

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(NotaSalva.Informações.identificação.DataHoraEmissão))
                {
                    var agora = DateTime.Now;
                    NotaSalva.Informações.identificação.DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                NotaSalva.Informações.identificação.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                NotaSalva.Informações.identificação.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (string.IsNullOrEmpty(NotaSalva.Informações.identificação.DataHoraSaídaEntrada))
                {
                    return DataEmissao;
                }
                return DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraSaídaEntrada);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraSaídaEntrada);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                NotaSalva.Informações.identificação.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraSaidaEntrada
        {
            get => DataSaidaEntrada.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraSaídaEntrada);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                NotaSalva.Informações.identificação.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public ObservableCollection<Municipio> MunicipiosIdentificacao { get; } = new ObservableCollection<Municipio>();
        public ushort EstadoIdentificacao
        {
            get => NotaSalva.Informações.identificação.CódigoUF;
            set
            {
                NotaSalva.Informações.identificação.CódigoUF = value;
                MunicipiosIdentificacao.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    MunicipiosIdentificacao.Add(item);
                }
            }
        }

        #endregion

        #region Transporte

        public ObservableCollection<Municipio> MunicipiosTransporte { get; } = new ObservableCollection<Municipio>();

        private Estado ufEscolhida;
        public Estado UFEscolhida
        {
            get
            {
                if (NotaSalva.Informações.transp.RetTransp.CMunFG != 0 && ufEscolhida == null)
                {
                    foreach (var item in Estados.EstadosCache)
                    {
                        var lista = Municipios.Get(item);
                        if (lista.Count(x => x.Codigo == NotaSalva.Informações.transp.RetTransp.CMunFG) > 0)
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

        public ObservableCollection<ModalidadesTransporte> Modalidades => ExtensoesPrincipal.ObterItens<ModalidadesTransporte>();

        public ModalidadesTransporte ModFrete
        {
            get => (ModalidadesTransporte)NotaSalva.Informações.transp.ModFrete;
            set => NotaSalva.Informações.transp.ModFrete = (ushort)value;
        }

        #endregion

        Popup Log = Popup.Current;
        AnalisadorNFe Analisador { get; set; }
        OperacoesNotaSalva OperacoesNota { get; set; }

        void Confirmar()
        {
            try
            {
                if (new ValidarDados(new ValidadorEmitente(NotaSalva.Informações.emitente),
                    new ValidadorDestinatario(NotaSalva.Informações.destinatário)).ValidarTudo(Log))
                {
                    NotaSalva.Informações.AtualizarChave();
                    Analisador.Normalizar();
                    Log.Escrever(TitulosComuns.ValidaçãoConcluída, "A nota fiscal foi validada. Aparentemente, não há irregularidades");
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        #region ColecoesExibicaoView

        ObservableCollection<DetalhesProdutos> Produtos { get; set; }
        ObservableCollection<Reboque> Reboques { get; }
        ObservableCollection<Volume> Volumes { get; }
        ObservableCollection<Duplicata> Duplicatas { get; }
        ObservableCollection<FornecimentoDiario> FornecimentosDiarios { get; }
        ObservableCollection<Deducoes> Deducoes { get; }
        ObservableCollection<Observacao> Observacoes { get; }
        ObservableCollection<ProcessoReferenciado> ProcessosReferenciados { get; }

        #endregion

        #region Adição e remoção básica

        void AdicionarProduto()
        {
            var detCompleto = new DetalhesProdutos
            {
                Produto = ProdutoSelecionado != null ? ProdutoSelecionado.ToProdutoOuServico() : new ProdutoOuServico()
            };
            MainPage.Current.Navegar<ManipulacaoProdutoCompleto>(detCompleto);
        }

        void EditarProduto(DetalhesProdutos produto)
        {
            MainPage.Current.Navegar<ManipulacaoProdutoCompleto>(produto);
        }

        void RemoverProduto(DetalhesProdutos produto)
        {
            NotaSalva.Informações.produtos.Remove(produto);
            Produtos.Remove(produto);
        }

        async void AdicionarNFeReferenciada()
        {
            var caixa = new CaixasDialogoNFe.AdicionarReferenciaEletronica();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = new DocumentoFiscalReferenciado
                {
                    RefNFe = caixa.Chave
                };
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(novo);
                NFesReferenciadas.Add(novo);
            }
        }

        async void AdicionarNFReferenciada()
        {
            var caixa = new CaixasDialogoNFe.AdicionarNF1AReferenciada();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var contexto = (NF1AReferenciada)caixa.DataContext;
                var novo = new DocumentoFiscalReferenciado
                {
                    RefNF = contexto
                };
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(novo);
                NFsReferenciadas.Add(novo);
            }
        }

        void RemoverDocReferenciado(DocumentoFiscalReferenciado doc)
        {
            NotaSalva.Informações.identificação.DocumentosReferenciados.Remove(doc);
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
            var add = new CaixasDialogoNFe.AdicionarReboque();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = add.DataContext as Reboque;
                NotaSalva.Informações.transp.Reboque.Add(novo);
                Reboques.Add(novo);
            }
        }

        void RemoverReboque(Reboque reboque)
        {
            NotaSalva.Informações.transp.Reboque.Remove(reboque);
            Reboques.Remove(reboque);
        }

        async void AdicionarVolume()
        {
            var add = new CaixasDialogoNFe.AdicionarVolume();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = add.DataContext as Volume;
                NotaSalva.Informações.transp.Vol.Add(novo);
                Volumes.Add(novo);
            }
        }

        void RemoverVolume(Volume volume)
        {
            NotaSalva.Informações.transp.Vol.Remove(volume);
            Volumes.Remove(volume);
        }

        async void AdicionarDuplicata()
        {
            var caixa = new CaixasDialogoNFe.AdicionarDuplicata();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.DataContext as Duplicata;
                NotaSalva.Informações.cobr.Dup.Add(novo);
                Duplicatas.Add(novo);
            }
        }

        void RemoverDuplicata(Duplicata duplicata)
        {
            NotaSalva.Informações.cobr.Dup.Remove(duplicata);
            Duplicatas.Remove(duplicata);
        }

        async void AdicionarFornecimento()
        {
            var caixa = new CaixasDialogoNFe.AdicionarFornecimentoDiario();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.DataContext as FornecimentoDiario;
                NotaSalva.Informações.cana.ForDia.Add(novo);
                FornecimentosDiarios.Add(novo);
            }
        }

        void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            NotaSalva.Informações.cana.ForDia.Remove(fornecimento);
            FornecimentosDiarios.Remove(fornecimento);
        }

        async void AdicionarDeducao()
        {
            var caixa = new CaixasDialogoNFe.AdicionarDeducao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.DataContext as Deducoes;
                NotaSalva.Informações.cana.Deduc.Add(novo);
                Deducoes.Add(novo);
            }
        }

        void RemoverDeducao(Deducoes deducao)
        {
            NotaSalva.Informações.cana.Deduc.Remove(deducao);
            Deducoes.Remove(deducao);
        }

        async void AdicionarObsContribuinte()
        {
            var caixa = new CaixasDialogoNFe.AdicionarObservacaoContribuinte();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = (Observacao)caixa.DataContext;
                NotaSalva.Informações.infAdic.ObsCont.Add(novo);
                Observacoes.Add(novo);
            }
        }

        void RemoverObsContribuinte(Observacao obs)
        {
            NotaSalva.Informações.infAdic.ObsCont.Remove(obs);
            Observacoes.Remove(obs);
        }

        async void AdicionarProcReferenciado()
        {
            var caixa = new CaixasDialogoNFe.AdicionarProcessoReferenciado();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novo = caixa.Item;
                NotaSalva.Informações.infAdic.ProcRef.Add(novo);
                ProcessosReferenciados.Add(novo);
            }
        }

        void RemoverProcReferenciado(ProcessoReferenciado proc)
        {
            NotaSalva.Informações.infAdic.ProcRef.Remove(proc);
            ProcessosReferenciados.Remove(proc);
        }

        #endregion

        async Task<bool> IValida.Verificar()
        {
            var retorno = true;
            var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, Cancelar.", "Atenção");
            mensagem.Commands.Add(new UICommand("Sair"));
            mensagem.Commands.Add(new UICommand("Cancelar", x => retorno = false));
            await mensagem.ShowAsync();
            return retorno;
        }

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

        private void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            AdicionarProduto();
        }

        private void EditarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            EditarProduto((DetalhesProdutos)contexto);
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            RemoverProduto((DetalhesProdutos)contexto);
        }
    }
}
