using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using BaseGeral.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.People, "Gerenciar fornecedores")]
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
            MainPage.Current.Navegar<AdicionarFornecedor>();
        }

        private void EditarFornecedor(object sender, RoutedEventArgs e)
        {
            var fornecedor = (FornecedorDI)((FrameworkElement)sender).DataContext;
            MainPage.Current.Navegar<AdicionarFornecedor>(fornecedor);
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Fornecedores.Buscar(busca);
        }
    }
}
