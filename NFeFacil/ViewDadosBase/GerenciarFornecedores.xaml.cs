using BaseGeral.Buscador;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarFornecedores : Page
    {
        BuscadorFornecedores Fornecedores { get; }

        public GerenciarFornecedores()
        {
            InitializeComponent();
            Fornecedores = new BuscadorFornecedores();
        }

        private void AdicionarFornecedor(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarComprador>();
        }

        private void EditarFornecedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            MainPage.Current.Navegar<AdicionarComprador>(((ExibicaoComprador)contexto).Root);
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Fornecedores.Buscar(busca);
        }
    }
}
