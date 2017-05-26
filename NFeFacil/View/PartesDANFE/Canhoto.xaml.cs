using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class Canhoto : UserControl
    {
        public GridLength Coluna0 => DimensoesPadrao.CentimeterToLength(4.5);
        public GridLength Coluna1 => DimensoesPadrao.CentimeterToLength(10);
        public GridLength Coluna2 => DimensoesPadrao.CentimeterToLength(4.5);

        public Canhoto()
        {
            InitializeComponent();
        }
    }

    public sealed class DimensoesPadrao
    {
        public Thickness PaddingCampoPadrao => new Thickness(CentimeterToPixel(0.3), CentimeterToPixel(0.15), CentimeterToPixel(0.15), 0);
        public GridLength AlturaLinhaPadrao
        {
            get
            {
                var retorno = CentimeterToLength(0.85);
                return retorno;
            }
        }

        static double CentimeterToPixel(double Centimeter)
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
