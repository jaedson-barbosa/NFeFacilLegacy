using BaseGeral.View;
using Fiscal.Certificacao.LAN;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        async void ConectarServidor(object sender, RoutedEventArgs e)
        {
            await InformacoesConexao.Cadastrar();
        }

        void EsquecerServidor(object sender, RoutedEventArgs e)
        {
            InformacoesConexao.Esquecer();
        }
    }
}
