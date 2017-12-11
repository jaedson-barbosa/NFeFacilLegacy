using NFeFacil.Log;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Informacoes : Page
    {
        public Informacoes()
        {
            InitializeComponent();
        }

        async void Doar10(object sender, TappedRoutedEventArgs e)
        {
            grdCompra.IsEnabled = false;
            if (await new ComprasInApp(ComprasInApp.Compras.Doacao10).Comprar())
            {
                Popup.Current.Escrever(TitulosComuns.Sucesso, "Muito obrigado pela contribuição.");
            }
            grdCompra.IsEnabled = true;
        }

        async void Doar25(object sender, TappedRoutedEventArgs e)
        {
            grdCompra.IsEnabled = false;
            if (await new ComprasInApp(ComprasInApp.Compras.Doacao25).Comprar())
            {
                Popup.Current.Escrever(TitulosComuns.Sucesso, "Muito obrigado pela contribuição.");
            }
            grdCompra.IsEnabled = true;
        }
    }
}
