using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class CampoTransporte : UserControl
    {
        GridLength Coluna00 => DimensoesPadrao.CentimeterToLength(6.5);
        GridLength Coluna01 => DimensoesPadrao.CentimeterToLength(3);
        GridLength Coluna02 => DimensoesPadrao.CentimeterToLength(2.5);
        GridLength Coluna03 => DimensoesPadrao.CentimeterToLength(2.5);

        GridLength Coluna10 => DimensoesPadrao.CentimeterToLength(8.5);
        GridLength Coluna11 => DimensoesPadrao.CentimeterToLength(6);

        GridLength Coluna4 => DimensoesPadrao.CentimeterToLength(1);
        GridLength Coluna5 => DimensoesPadrao.CentimeterToLength(3.5);

        public CampoTransporte()
        {
            InitializeComponent();
        }
    }
}
