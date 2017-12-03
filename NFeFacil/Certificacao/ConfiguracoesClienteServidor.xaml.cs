using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Certificacao
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ConfiguracoesClienteServidor : Page
    {
        public ConfiguracoesClienteServidor()
        {
            InitializeComponent();
        }

        async void ConectarServidor(object sender, TappedRoutedEventArgs e)
        {
            await LAN.InformacoesConexao.Cadastrar();
        }

        void EsquecerServidor(object sender, TappedRoutedEventArgs e)
        {
            LAN.InformacoesConexao.Esquecer();
        }
    }
}
