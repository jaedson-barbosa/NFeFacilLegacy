using NFeFacil.Sincronizacao;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class PrimeiroUso : Page
    {
        public PrimeiroUso()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                using (var db = new AplicativoContext())
                {
                    if (db.Emitentes.Count() > 0)
                    {
                        await Task.Delay(500);
                        MainPage.Current.Navegar<EscolhaEmitente>();
                    }
                }
            }
        }

        void Manualmente(object sender, TappedRoutedEventArgs e) => MainPage.Current.Navegar<AdicionarEmitente>();
        void Sincronizar(object sender, TappedRoutedEventArgs e) => MainPage.Current.Navegar<SincronizacaoCliente>();
        async void RestaurarBackup(object sender, TappedRoutedEventArgs e)
        {
            if (await Backup.RestaurarBackup())
            {
                await Task.Delay(500);
                MainPage.Current.Navegar<EscolhaEmitente>();
            }
        }
    }
}
