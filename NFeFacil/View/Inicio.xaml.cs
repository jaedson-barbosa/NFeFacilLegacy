using NFeFacil.Certificacao;
using NFeFacil.Importacao;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.Sincronizacao;
using NFeFacil.Validacao;
using NFeFacil.ViewDadosBase;
using NFeFacil.ViewRegistroVenda;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inicio : Page
    {
        public Inicio()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            grdPrincipal.SelectedIndex = -1;
        }

        private async void grdPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            switch ((e.AddedItems[0] as FrameworkElement).Tag)
            {
                case "GerenciarDadosBase":
                    MainPage.Current.Navegar<GerenciarDadosBase>();
                    break;
                case "ManipulacaoRegistroVenda":
                    MainPage.Current.Navegar<ManipulacaoRegistroVenda>();
                    break;
                case "CriadorNFe":
                    if (await new ViewNFe.CriadorNFe().ShowAsync() == ContentDialogResult.Secondary)
                    {
                        grdPrincipal.SelectedIndex = -1;
                    }
                    break;
                case "CriarNFeEntrada":
                    if (!await CriarNFeEntrada())
                    {
                        grdPrincipal.SelectedIndex = -1;
                    }
                    break;
                case "NotasSalvas":
                    MainPage.Current.Navegar<ViewNFe.NotasSalvas>();
                    break;
                case "RegistrosVenda":
                    MainPage.Current.Navegar<RegistrosVenda>();
                    break;
                case "Consulta":
                    MainPage.Current.Navegar<Consulta>();
                    break;
                case "VendasAnuais":
                    MainPage.Current.Navegar<VendasAnuais>();
                    break;
                case "Configuracoes":
                    MainPage.Current.Navegar<Configuracoes>();
                    break;
                case "Certificado":
                    switch (ConfiguracoesCertificacao.Origem)
                    {
                        case OrigemCertificado.Importado:
                            MainPage.Current.Navegar<ConfiguracoesCertificadoImportado>();
                            break;
                        case OrigemCertificado.Servidor:
                            MainPage.Current.Navegar<ConfiguracoesServidorCertificacao>();
                            break;
                        case OrigemCertificado.Cliente:
                            MainPage.Current.Navegar<ConfiguracoesClienteServidor>();
                            break;
                    }
                    break;
                case "ImportacaoDados":
                    MainPage.Current.Navegar<ImportacaoDados>();
                    break;
                case "Sincronizacao":
                    if (ConfiguracoesSincronizacao.Tipo == TipoAppSincronizacao.Cliente)
                    {
                        MainPage.Current.Navegar<SincronizacaoCliente>();
                    }
                    else
                    {
                        MainPage.Current.Navegar<SincronizacaoServidor>();
                    }
                    break;
                case "Informacoes":
                    MainPage.Current.Navegar<Informacoes>();
                    break;
                default:
                    break;
            }
        }

        async Task<bool> CriarNFeEntrada()
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".xml");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                try
                {
                    var xml = await ImportarNotaFiscal.ObterXML(arq);
                    var proc = xml.FromXElement<Processo>();
                    var nfe = proc.NFe;
                    if (nfe.Informacoes.destinatário.CNPJ == Propriedades.EmitenteAtivo.CNPJ)
                    {
                        using (var repo = new Repositorio.MEGACLASSE())
                        {
                            var c = repo.ObterClienteViaCNPJ(nfe.Informacoes.emitente.CNPJ);
                            if (c != null)
                            {
                                nfe.Informacoes.destinatário = c.ToDestinatario();
                                nfe.Informacoes.emitente = Propriedades.EmitenteAtivo.ToEmitente();
                                nfe.Informacoes.identificacao.TipoOperacao = 0;
                                var analisador = new AnalisadorNFe(ref nfe);
                                analisador.Desnormalizar();
                                if (await new ViewNFe.CriadorNFe(nfe).ShowAsync() == ContentDialogResult.Primary)
                                {
                                    Popup.Current.Escrever(TitulosComuns.Sucesso, "Nota de entrada criada. Agora verifique se todas as informações estão corretas.");
                                    return true;
                                }
                            }
                            else
                            {
                                Popup.Current.Escrever(TitulosComuns.Atenção, "Para uma melhor esperiência na edição da NFe é preciso cadastrar o emitente da nota fiscal como cliente.\r\n" +
                                    "Após concluir o cadastro tente novamente criar a nota de entrada.");
                                var di = new ClienteDI(nfe.Informacoes.emitente);
                                MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>(di);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        Popup.Current.Escrever(TitulosComuns.Atenção, "O cliente dessa nota fiscal não é o emitente ativo, por favor, escolha apenas notas fiscais para o emitente ativo.");
                    }
                }
                catch (Exception erro)
                {
                    erro.ManipularErro();
                }
            }
            return false;
        }
    }
}
