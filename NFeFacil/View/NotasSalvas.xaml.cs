using Windows.UI.Xaml.Controls;
using NFeFacil.ViewModel;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class NotasSalvas : Page
    {
        public NotasSalvas()
        {
            InitializeComponent();
            DataContext = new NotasSalvasDataContext(ref lstNotas);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Current.SeAtualizar(Symbol.Library, "Notas salvas");
        }
    }
}
