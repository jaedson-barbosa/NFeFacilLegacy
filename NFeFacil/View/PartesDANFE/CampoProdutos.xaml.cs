using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class CampoProdutos : UserControl
    {
        public CampoProdutos()
        {
            this.InitializeComponent();
        }
    }

    public sealed class DimensoesCampoProdutos
    {
        public GridLength Coluna0 => DimensoesPadrao.CentimeterToLength(1.5);
        public GridLength Coluna1 => DimensoesPadrao.CentimeterToLength(4.75);
        public GridLength Coluna2 => DimensoesPadrao.CentimeterToLength(1.25);
        public GridLength ColunaGeral3 => DimensoesPadrao.CentimeterToLength(1.5);
        public GridLength ColunaGeral4 => DimensoesPadrao.CentimeterToLength(2);
        public GridLength ColunaGeral5 => DimensoesPadrao.CentimeterToLength(6.5);
        public GridLength ColunaGeral6 => DimensoesPadrao.CentimeterToLength(1.5);

        public GridLength LinhaPadrao => DimensoesPadrao.CentimeterToLength(0.55);
    }
}
