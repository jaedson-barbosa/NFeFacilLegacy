using NFeFacil.Fiscal.ViewNFe.PacotesDANFE;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static BaseGeral.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Fiscal.ViewNFe.PartesDANFE
{
    public sealed partial class CampoTransporte : UserControl
    {
        public DadosMotorista Contexto { get; set; }

        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();

        GridLength Coluna00 => CMToLength(6.5);
        GridLength Coluna01 => CMToLength(3);
        GridLength Coluna02 => CMToLength(2.5);
        GridLength Coluna03 => CMToLength(2.5);

        GridLength Coluna10 => CMToLength(8.5);
        GridLength Coluna11 => CMToLength(6);

        GridLength Coluna4 => CMToLength(1);
        GridLength Coluna5 => CMToLength(3.5);

        public CampoTransporte()
        {
            InitializeComponent();
        }
    }
}
