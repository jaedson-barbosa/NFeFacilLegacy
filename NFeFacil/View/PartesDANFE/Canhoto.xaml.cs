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
}
