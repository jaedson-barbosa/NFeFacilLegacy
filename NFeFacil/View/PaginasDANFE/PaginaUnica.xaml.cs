using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PaginasDANFE
{
    public sealed partial class PaginaUnica : UserControl
    {
        double LarguraPagina => CentimeterToPixel(21);
        double AlturaPagina => CentimeterToPixel(29.7);

        Thickness MargemPadrao => new Thickness(CentimeterToPixel(1));

        public PaginaUnica()
        {
            this.InitializeComponent();
        }

        static double CentimeterToPixel(double Centimeter)
        {
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }
    }
}
