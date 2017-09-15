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
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarNFeReferenciada()
        {
            var caixa = new CaixasDialogoNFe.AdicionarReferenciaEletronica();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(new DocumentoFiscalReferenciado
                {
                    RefNFe = caixa.Chave
                });
                OnPropertyChanged(nameof(NFesReferenciadas));
            }
        }

        async void AdicionarNFReferenciada()
        {
            var caixa = new CaixasDialogoNFe.AdicionarNF1AReferenciada();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var contexto = (NF1AReferenciada)caixa.DataContext;
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(new DocumentoFiscalReferenciado
                {
                    RefNF = contexto
                });
                OnPropertyChanged(nameof(NFsReferenciadas));
            }
        }

        void RemoverDocReferenciado(DocumentoFiscalReferenciado doc)
        {
            NotaSalva.Informações.identificação.DocumentosReferenciados.Remove(doc);
            OnPropertyChanged(nameof(NFesReferenciadas), nameof(NFsReferenciadas));
        }

        async void AdicionarReboque()
        {
            var add = new CaixasDialogoNFe.AdicionarReboque();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.transp.Reboque.Add(add.DataContext as Reboque);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverReboque(Reboque reboque)
        {
            NotaSalva.Informações.transp.Reboque.Remove(reboque);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarVolume()
        {
            var add = new CaixasDialogoNFe.AdicionarVolume();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.transp.Vol.Add(add.DataContext as Volume);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverVolume(Volume volume)
        {
            NotaSalva.Informações.transp.Vol.Remove(volume);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarDuplicata()
        {
            var caixa = new CaixasDialogoNFe.AdicionarDuplicata();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.cobr.Dup.Add(caixa.DataContext as Duplicata);
                OnPropertyChanged("Cobranca");
            }
        }

        void RemoverDuplicata(Duplicata duplicata)
        {
            NotaSalva.Informações.cobr.Dup.Remove(duplicata);
            OnPropertyChanged("Cobranca");
        }

        async void AdicionarFornecimento()
        {
            var caixa = new CaixasDialogoNFe.AdicionarFornecimentoDiario();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.cana.ForDia.Add(caixa.DataContext as FornecimentoDiario);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            NotaSalva.Informações.cana.ForDia.Remove(fornecimento);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarDeducao()
        {
            var caixa = new CaixasDialogoNFe.AdicionarDeducao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.cana.Deduc.Add(caixa.DataContext as Deducoes);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverDeducao(Deducoes deducao)
        {
            NotaSalva.Informações.cana.Deduc.Remove(deducao);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarObsContribuinte()
        {
            var caixa = new CaixasDialogoNFe.AdicionarObservacaoContribuinte();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.infAdic.ObsCont.Add((Observacao)caixa.DataContext);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverObsContribuinte(Observacao obs)
        {
            NotaSalva.Informações.infAdic.ObsCont.Remove(obs);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarProcReferenciado()
        {
            var caixa = new CaixasDialogoNFe.AdicionarProcessoReferenciado();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.infAdic.ProcRef.Add(caixa.Item);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverProcReferenciado(ProcessoReferenciado proc)
        {
            NotaSalva.Informações.infAdic.ProcRef.Remove(proc);
            OnPropertyChanged(nameof(NotaSalva));
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
    }
}
