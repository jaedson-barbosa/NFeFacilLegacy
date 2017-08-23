using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class DadosNFe : UserControl
    {
        GridLength Coluna0 => DimensoesPadrao.CentimeterToLength(8);
        GridLength Coluna1 => DimensoesPadrao.CentimeterToLength(3);
        GridLength Coluna2 => DimensoesPadrao.CentimeterToLength(8);

        GridLength Linha0 => DimensoesPadrao.CentimeterToLength(3.4);

        public DadosNFe()
        {
            InitializeComponent();
        }
    }
}
