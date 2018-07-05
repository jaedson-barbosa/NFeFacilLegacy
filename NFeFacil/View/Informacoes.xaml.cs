using BaseGeral.Log;
using BaseGeral.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    [DetalhePagina("\uE946", "Informações")]
    public sealed partial class Informacoes : Page
    {
        public Informacoes()
        {
            InitializeComponent();
        }

        async void Doar10(object sender, RoutedEventArgs e)
        {
            grdCompra.IsEnabled = false;
            if (await ComprasInApp.Comprar(Compras.Doacao10))
            {
                Popup.Current.Escrever(TitulosComuns.Sucesso, "Muito obrigado pela contribuição.");
            }
            grdCompra.IsEnabled = true;
        }

        async void Doar25(object sender, RoutedEventArgs e)
        {
            grdCompra.IsEnabled = false;
            if (await ComprasInApp.Comprar(Compras.Doacao25))
            {
                Popup.Current.Escrever(TitulosComuns.Sucesso, "Muito obrigado pela contribuição.");
            }
            grdCompra.IsEnabled = true;
        }
    }
}
