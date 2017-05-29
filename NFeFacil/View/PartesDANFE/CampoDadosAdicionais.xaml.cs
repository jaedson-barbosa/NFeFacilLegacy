using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class CampoDadosAdicionais : UserControl
    {
        GridLength AlturaCampo => DimensoesPadrao.CentimeterToLength(3.2);

        public CampoDadosAdicionais()
        {
            this.InitializeComponent();
        }
    }
}
