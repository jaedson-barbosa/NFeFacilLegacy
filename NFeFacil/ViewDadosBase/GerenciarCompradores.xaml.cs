using BaseGeral;
using BaseGeral.Buscador;
using BaseGeral.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.Manage, "Gerenciar compradores")]
    public sealed partial class GerenciarCompradores : Page
    {
        BuscadorComprador Compradores { get; }

        public GerenciarCompradores()
        {
            InitializeComponent();
            Compradores = new BuscadorComprador();
        }

        private void AdicionarComprador(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarComprador>();
        }

        private void EditarComprador(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            MainPage.Current.Navegar<AdicionarComprador>(((ExibicaoComprador)contexto).Root);
        }

        private void InativarComprador(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var compr = (ExibicaoComprador)contexto;

            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(compr.Root, DefinicoesTemporarias.DateTimeNow);
                Compradores.Remover(compr);
            }
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Compradores.Buscar(busca);
        }
    }
}
