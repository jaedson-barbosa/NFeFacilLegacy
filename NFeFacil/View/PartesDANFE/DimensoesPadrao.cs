using Windows.UI.Xaml;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed class DimensoesPadrao
    {
        public GridLength AlturaLinhaPadrao => CentimeterToLength(0.85);
        public double AlturaLinha => AlturaLinhaPadrao.Value;
        public Thickness MargemBloco => new Thickness(0, 0, 0, CentimeterToPixel(0.1));
        public double LarguraTotal => LarguraTotalStatic;
        internal static double LarguraTotalStatic => CentimeterToPixel(19);

        internal static double CentimeterToPixel(double Centimeter)
        {
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }

        internal static GridLength CentimeterToLength(double Centimeter)
        {
            return new GridLength(CentimeterToPixel(Centimeter));
        }
    }
}
