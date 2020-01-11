using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    [BaseGeral.View.DetalhePagina(Symbol.People, "Cliente")]
    public sealed partial class AdicionarCliente : Page
    {
        ControllerAdicaoClienteGeral Controller;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Controller = (ControllerAdicaoClienteGeral)e.Parameter;
            InitializeComponent();
        }

        void Confirmar_Click(object sender, RoutedEventArgs e) => Controller.Confirmar();
        void Cancelar_Click(object sender, RoutedEventArgs e) => Controller.Cancelar();
    }
}