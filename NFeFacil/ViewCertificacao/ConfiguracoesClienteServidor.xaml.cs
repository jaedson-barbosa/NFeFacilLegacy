using BaseGeral.Certificacao.LAN;
using BaseGeral.View;
using Fiscal.Certificacao.LAN;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Certificacao
{
    [DetalhePagina(Symbol.Permissions, "Certificação")]
    public sealed partial class ConfiguracoesClienteServidor : Page
    {
        public ConfiguracoesClienteServidor()
        {
            InitializeComponent();
        }

        async void ConectarServidor(object sender, TappedRoutedEventArgs e)
        {
            await InformacoesConexao.Cadastrar();
        }

        void EsquecerServidor(object sender, TappedRoutedEventArgs e)
        {
            InformacoesConexao.Esquecer();
        }
    }
}
