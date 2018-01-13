using NFeFacil.ViewNFe.DANFE.Pacotes;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.ViewNFe.DANFE.PartesDANFE
{
    public sealed partial class CampoISSQN : UserControl
    {
        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();
        public DadosISSQN Contexto { get; set; }
        string TotalServicos => Contexto.TotalServiços;

        public CampoISSQN()
        {
            InitializeComponent();
        }
    }
}
