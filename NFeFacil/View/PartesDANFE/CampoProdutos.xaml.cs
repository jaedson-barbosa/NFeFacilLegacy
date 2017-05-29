using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class CampoProdutos : UserControl
    {
        GridLength Coluna0 => DimensoesPadrao.CentimeterToLength(1.5);
        GridLength Coluna1 => DimensoesPadrao.CentimeterToLength(4.75);
        GridLength Coluna2 => DimensoesPadrao.CentimeterToLength(1.25);
        GridLength ColunaGeral3 => DimensoesPadrao.CentimeterToLength(1.5);
        GridLength ColunaGeral4 => DimensoesPadrao.CentimeterToLength(2);
        GridLength ColunaGeral5 => DimensoesPadrao.CentimeterToLength(6.5);
        GridLength ColunaGeral6 => DimensoesPadrao.CentimeterToLength(1.5);

        public CampoProdutos()
        {
            this.InitializeComponent();
            DataContext = this;
        }
    }
}
