using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class Canhoto : UserControl
    {
        GridLength ColunaMeio => DimensoesPadrao.CentimeterToLength(10);

        public Canhoto()
        {
            InitializeComponent();
        }
    }
}
