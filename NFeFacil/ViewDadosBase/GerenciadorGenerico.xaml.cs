using BaseGeral.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    [DetalhePagina(Symbol.Manage, "Gerenciar dados")]
    public sealed partial class GerenciadorGenerico : Page
    {
        public static string AcaoSecundariaLabel { get; private set; }
        ControllerGerenciadorGeral Controller;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Controller = (ControllerGerenciadorGeral)e.Parameter;
            AcaoSecundariaLabel = Controller.AcaoSecundariaLabel;
            InitializeComponent();
        }

        void Editar(object sender, RoutedEventArgs e)
        {
            var contexto = (ExibicaoGenerica)((FrameworkElement)sender).DataContext;
            Controller.Editar(contexto);
        }

        void AcaoSecundaria(object sender, RoutedEventArgs e)
        {
            var contexto = (ExibicaoGenerica)((FrameworkElement)sender).DataContext;
            Controller.AcaoSecundaria(contexto);
        }

        void Adicionar(object sender, RoutedEventArgs e) => Controller.Adicionar();
        void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Controller.Buscar(busca);
        }
    }
}
