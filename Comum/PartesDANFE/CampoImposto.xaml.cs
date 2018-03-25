using Comum.PacotesDANFE;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Comum.PartesDANFE
{
    public sealed partial class CampoImposto : UserControl
    {
        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();
        public DadosImposto Contexto { get; set; }

        public CampoImposto()
        {
            InitializeComponent();
        }
    }
}
