using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inicio : Page, IEsconde
    {
        public Inicio()
        {
            InitializeComponent();
            Propriedades.Intercambio.SeAtualizar(Telas.Inicio, Symbol.Home, nameof(Inicio));
        }

        private async void AbrirFunção(object sender, TappedRoutedEventArgs e)
        {
            await Propriedades.Intercambio.AbrirFunçaoAsync((sender as FrameworkElement).Name);
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
