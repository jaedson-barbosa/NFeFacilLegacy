using NFeFacil.Importacao;
using NFeFacil.Sincronizacao;
using NFeFacil.ViewDadosBase;
using NFeFacil.ViewRegistroVenda;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
            MainPage.Current.SeAtualizar(Symbol.Home, nameof(Inicio));
        }

        private async void AbrirFunção(object sender, TappedRoutedEventArgs e)
        {
            switch ((sender as FrameworkElement).Name)
            {
                case "GerenciarDadosBase":
                    MainPage.Current.Navegar<GerenciarDadosBase>();
                    break;
                case "ControleEstoque":
                    MainPage.Current.Navegar<ControleEstoque>();
                    break;
                case "ManipulacaoRegistroVenda":
                    MainPage.Current.Navegar<ManipulacaoRegistroVenda>();
                    break;
                case "CriadorNFe":
                    await new ViewNFe.CriadorNFe().ShowAsync();
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
                case "ConfiguracoesCertificado":
                    MainPage.Current.Navegar<ConfiguracoesCertificado>();
                    break;
                case "ImportacaoDados":
                    MainPage.Current.Navegar<ImportacaoDados>();
                    break;
                case "ConfigSincronizacao":
                    if (ConfiguracoesSincronizacao.Tipo == TipoAppSincronizacao.Cliente)
                    {
                        MainPage.Current.Navegar<SincronizacaoCliente>();
                    }
                    else
                    {
                        MainPage.Current.Navegar<SincronizacaoServidor>();
                    }
                    break;
                case "Backup":
                    MainPage.Current.Navegar<Backup>();
                    break;
                default:
                    break;
            }
        }
    }
}
