using BibliotecaCentral;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class RegistrosVenda : Page
    {
        public RegistrosVenda()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                lstVendas.ItemsSource = db.Vendas.GerarObs();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Library, "Vendas");
        }

        private void Exibir(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            var venda = (RegistrosVenda)item.DataContext;
            var conjunto = new GrupoViewBanco<RegistrosVenda>
            {
                ItemBanco = venda,
                OperacaoRequirida = TipoOperacao.Edicao
            };
            MainPage.Current.AbrirFunçao(typeof(ManipulacaoRegistroVenda), conjunto);
        }
    }
}
