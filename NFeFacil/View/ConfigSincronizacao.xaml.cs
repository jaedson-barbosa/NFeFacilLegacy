using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ConfigSincronizacao : Page, IEsconde
    {
        public ConfigSincronizacao()
        {
            InitializeComponent();
            MainPage.Current.SeAtualizar(Telas.ConfigSincronizacao, Symbol.Sync, "Config. de sincronização");
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
