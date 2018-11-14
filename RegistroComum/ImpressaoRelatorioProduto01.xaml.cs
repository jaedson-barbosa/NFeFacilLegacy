using BaseGeral.View;
using Venda;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum
{
    [DetalhePagina("\uE749", "Relatório gerado")]
    public sealed partial class ImpressaoRelatorioProduto01 : Page
    {
        readonly GerenciadorImpressao Gerenciador = new GerenciadorImpressao();

        public ImpressaoRelatorioProduto01()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dados = (DadosRelatorioProduto01)e.Parameter;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Gerenciador.Dispose();
        }

        async void Imprimir(object sender, RoutedEventArgs e)
        {
            await Gerenciador.Imprimir(ConteinerPaginas.Children);
        }
    }
}
