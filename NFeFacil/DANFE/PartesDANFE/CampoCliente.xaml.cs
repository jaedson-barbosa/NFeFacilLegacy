using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class CampoCliente : UserControl
    {
        GridLength Coluna00 => DimensoesPadrao.CentimeterToLength(12.5);
        GridLength Coluna01 => DimensoesPadrao.CentimeterToLength(3.5);

        GridLength Coluna10 => DimensoesPadrao.CentimeterToLength(8.5);
        GridLength Coluna11 => DimensoesPadrao.CentimeterToLength(5);
        GridLength Coluna12 => DimensoesPadrao.CentimeterToLength(2.5);

        GridLength Coluna20 => DimensoesPadrao.CentimeterToLength(8.5);
        GridLength Coluna21 => DimensoesPadrao.CentimeterToLength(3);
        GridLength Coluna22 => DimensoesPadrao.CentimeterToLength(1);
        GridLength Coluna23 => DimensoesPadrao.CentimeterToLength(3.5);

        GridLength Coluna3 => DimensoesPadrao.CentimeterToLength(3);

        public CampoCliente()
        {
            InitializeComponent();
        }
    }
}
