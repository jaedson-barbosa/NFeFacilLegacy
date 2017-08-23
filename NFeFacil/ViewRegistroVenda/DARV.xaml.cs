using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DARV : Page
    {
        public DARV()
        {
            this.InitializeComponent();
            DefinirTamanho(19, 27.7);
            paiGeral.Margin = new Thickness(CentimeterToPixel(1));
        }

        void DefinirTamanho(double largura, double altura)
        {
            paiGeral.Width = CentimeterToPixel(largura - 2);
            paiGeral.Height = CentimeterToPixel(altura - 2);
        }
    }

    public struct ConjuntoDadosDARV
    {
        
    }
}
