using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class CampoCliente : UserControl
    {
        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();

        GridLength Coluna00 => CentimeterToLength(12.5);
        GridLength Coluna01 => CentimeterToLength(3.5);

        GridLength Coluna10 => CentimeterToLength(8.5);
        GridLength Coluna11 => CentimeterToLength(5);
        GridLength Coluna12 => CentimeterToLength(2.5);

        GridLength Coluna20 => CentimeterToLength(8.5);
        GridLength Coluna21 => CentimeterToLength(3);
        GridLength Coluna22 => CentimeterToLength(1);
        GridLength Coluna23 => CentimeterToLength(3.5);

        GridLength Coluna3 => CentimeterToLength(3);

        public CampoCliente()
        {
            InitializeComponent();
        }
    }
}
