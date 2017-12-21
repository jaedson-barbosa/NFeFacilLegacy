using NFeFacil.ViewDadosBase.GerenciamentoProdutos;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [View.DetalhePagina(Symbol.Manage, "Dados base")]
    public sealed partial class GerenciarDadosBase : Page
    {
        public GerenciarDadosBase()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            grdPrincipal.SelectedIndex = -1;
        }

        private void grdPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            switch ((e.AddedItems[0] as FrameworkElement).Tag)
            {
                case "Clientes":
                    MainPage.Current.Navegar<GerenciarClientes>();
                    break;
                case "Motoristas":
                    MainPage.Current.Navegar<GerenciarMotoristas>();
                    break;
                case "Produtos":
                    MainPage.Current.Navegar<GerenciarProdutos>();
                    break;
                case "Vendedores":
                    MainPage.Current.Navegar<GerenciarVendedores>();
                    break;
                case "Compradores":
                    MainPage.Current.Navegar<GerenciarCompradores>();
                    break;
                default:
                    break;
            }
        }
    }
}
