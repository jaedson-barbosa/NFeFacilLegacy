using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.Validacao;
using NFeFacil.ViewModel.NotaFiscal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoNotaFiscal : Page, IEsconde, IValida, INotifyPropertyChanged
    {
        #region InformaçõesDaNota
        public IdentificacaoDataContext Dados { get; set; }
        public EmitenteDataContext Emitente { get; set; }
        public ClienteDataContext Destinatario { get; set; }
        public List<DetalhesProdutos> Produtos { get; set; }
        public ObservableCollection<DetalhesProdutos> ProdutosObservableCollection => Produtos.GerarObs();
        public TransporteDataContext TransporteNota { get; set; }
        public CobrancaDataContext Cobranca { get; set; }
        public InformacoesAdicionais InformacoesAdicionais { get; set; }
        public Exportacao Exportacao { get; set; }
        public Compra CompraNota { get; set; }
        public CanaDataContext Cana { get; set; }
        public Total Totais { get; set; }
        #endregion

        private Popup Log = new Popup();
        private NFe notaSalva;
        private Processo notaEmitida;

        private StatusNFe status = StatusNFe.EdiçãoCriação;
        private StatusNFe StatusAtual
        {
            get { return status; }
            set
            {
                AppBarButton[] botões =
                {
                    btnConfirmar, btnSalvar, btnAssinar, btnTransmitir, btnGerarDANFE
                };
                for (int i = 0; i < botões.Length; i++)
                {
                    var valor = (int)value;
                    botões[i].IsEnabled = i == (valor >= botões.Length ? botões.Length - 1 : valor);
                }
                status = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ManipulacaoNotaFiscal()
        {
            this.InitializeComponent();
            Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.NewFolder, "Emitir nova nota");
            using (var db = new AplicativoContext())
            {
                cmbDestinatarios.ItemsSource = db.Clientes
                    .Include(x => x.endereco)
                    .ToList();
                cmbEmitentes.ItemsSource = db.Emitentes
                    .Include(x => x.endereco)
                    .ToList();
                cmbProdutos.ItemsSource = db.Produtos
                    .ToList();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = (NotaComDados)e.Parameter;
            switch (param.tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.Add, "Criar nota fiscal");
                    break;
                case TipoOperacao.Edicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.Edit, "Editar nota fiscal");
                    break;
                default:
                    break;
            }
            Detalhes nfe;
            if (param.proc?.NFe != null)
            {
                nfe = param.proc.NFe.Informações;
                notaEmitida = param.proc;
            }
            else
            {
                nfe = param.nota.Informações;
                notaSalva = param.nota;
            }
            StatusAtual = (StatusNFe)param.dados.Status;
            Dados = new IdentificacaoDataContext(ref nfe.identificação);
            Emitente = new EmitenteDataContext(ref nfe.emitente);
            Destinatario = new ClienteDataContext(ref nfe.destinatário);
            Produtos = nfe.produtos;
            TransporteNota = new TransporteDataContext(ref nfe.transp);
            Cobranca = new CobrancaDataContext(ref nfe.cobr);
            InformacoesAdicionais = nfe.infAdic;
            Exportacao = nfe.exporta;
            CompraNota = nfe.compra;
            Cana = new CanaDataContext(ref nfe.cana);

            if (CoreApplication.Properties.TryGetValue("ProdutoPendente", out object temp))
            {
                var detalhes = (DetalhesProdutos)temp;
                detalhes.número = Produtos.Count + 1;
                Produtos.Add(detalhes);
                pvtPrincipal.SelectedIndex = 3;
                CoreApplication.Properties.Remove("ProdutoPendente");
            }

            DataContext = this;
            AttListasProdutos();
        }

        private void cmbEmitentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecionado = cmbEmitentes.SelectedValue as EmitenteDI;
            if (notaSalva != null)
            {
                notaSalva.Informações.emitente = selecionado;
                Emitente.Emit = notaSalva.Informações.emitente;
            }
            else
            {
                notaEmitida.NFe.Informações.emitente = selecionado;
                Emitente.Emit = notaEmitida.NFe.Informações.emitente;
            }
            Emitente.AttTudo();
        }

        private void cmbDestinatarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecionado = cmbDestinatarios.SelectedValue as ClienteDI;
            if (notaSalva != null)
            {
                notaSalva.Informações.destinatário = selecionado;
                Destinatario.Cliente = notaSalva.Informações.destinatário;
            }
            else
            {
                notaEmitida.NFe.Informações.destinatário = selecionado;
                Destinatario.Cliente = notaEmitida.NFe.Informações.destinatário;
            }
            Destinatario.AttTudo();
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (Validar())
            {
                notaSalva = new NFe
                {
                    Informações = new Detalhes
                    {
                        identificação = Dados.Ident,
                        emitente = new Emitente(Emitente.Emit),
                        destinatário = new Destinatario(Destinatario.Cliente),
                        transp = TransporteNota.ObterTranporteNormalizado(),
                        produtos = Produtos,
                        total = Totais,
                        cobr = ÉDefault(Cobranca.Cobranca),
                        infAdic = ÉDefault(InformacoesAdicionais),
                        exporta = ÉDefault(Exportacao),
                        compra = ÉDefault(CompraNota),
                        cana = ÉDefault(Cana.Cana)
                    }
                };
                Log.Escrever(TitulosComuns.ValidaçãoConcluída, "A nota fiscal foi validada. Aparentemente, não há irregularidades");
                StatusAtual = StatusNFe.Validado;
            }
        }

        private static T ÉDefault<T>(T valor) where T : class => valor != default(T) ? valor : null;

        private bool Validar()
        {
            return new ValidarDados(
                new ValidadorEmitente(Emitente.Emit),
                new ValidadorDestinatario(Destinatario.Cliente)).ValidarTudo(Log);
        }

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StatusAtual = StatusNFe.Salvo;
                PastaNotasFiscais pasta = new PastaNotasFiscais();
                var xml = notaSalva.ToXElement<NFe>();
                var di = NFeDI.Converter(xml);
                di.Status = (int)StatusAtual;
                using (var db = new AplicativoContext())
                {
                    await pasta.AdicionarOuAtualizar(xml, di.Id);
                    var quant = db.NotasFiscais.Count(x => x.Id == di.Id);
                    if (quant == 1) db.Update(di);
                    else db.Add(di);
                }
                Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso. Agora podes sair da aplicação sem perder esta NFe.");
            }
            catch (Exception erro)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, erro.Message);
                StatusAtual = StatusNFe.EdiçãoCriação;
            }
        }

        private void btnAssinar_Click(object sender, RoutedEventArgs e)
        {
            Log.Escrever(TitulosComuns.ErroCatastrófico, "Função ainda não implementada.");
            StatusAtual = StatusNFe.Assinado;
        }

        private void btnTransmitir_Click(object sender, RoutedEventArgs e)
        {
            Log.Escrever(TitulosComuns.ErroCatastrófico, "Função ainda não implementada.");
            StatusAtual = StatusNFe.Emitido;
        }

        private async void btnGerarDANFE_Click(object sender, RoutedEventArgs e)
        {
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ViewDANFE), notaEmitida);
            StatusAtual = StatusNFe.Impresso;
        }

        #region TelaProdutos
        private async void btnAdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            var selec = cmbProdutos.SelectedValue;
            var prod = selec == null ? new ProdutoDI() : selec as ProdutoDI;
            var detCompleto = new DetalhesProdutos
            {
                Produto = new ProdutoOuServico(prod)
            };
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoProdutoCompleto), detCompleto);
        }

        private void AttListasProdutos()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Produtos)));
            Totais = new Total(Produtos);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Totais)));
        }

        private void btnRemoverItemProdutos_Click(object sender, RoutedEventArgs e)
        {
            Produtos.Remove((sender as FrameworkElement).DataContext as DetalhesProdutos);
            AttListasProdutos();
        }
        #endregion

        async Task<bool> IValida.Verificar()
        {
            var retorno = true;
            if (StatusAtual == StatusNFe.EdiçãoCriação)
            {
                var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, Cancelar.", "Atenção");
                mensagem.Commands.Add(new UICommand("Sair"));
                mensagem.Commands.Add(new UICommand("Cancelar", x => retorno = false));
                await mensagem.ShowAsync();
            }
            return retorno;
        }

        async Task IEsconde.EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
