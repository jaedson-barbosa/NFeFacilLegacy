using NFeFacil.ViewNFe.DANFE.Pacotes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.ViewNFe.DANFE.PartesDANFE
{
    public sealed partial class CampoCliente : UserControl
    {
        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();

        GridLength Coluna00 => CMToLength(12.5);
        GridLength Coluna01 => CMToLength(3.5);

        GridLength Coluna10 => CMToLength(8.5);
        GridLength Coluna11 => CMToLength(5);
        GridLength Coluna12 => CMToLength(2.5);

        GridLength Coluna20 => CMToLength(8.5);
        GridLength Coluna21 => CMToLength(3);
        GridLength Coluna22 => CMToLength(1);
        GridLength Coluna23 => CMToLength(3.5);

        GridLength Coluna3 => CMToLength(3);

        public DadosCliente Contexto { get; set; }

        public CampoCliente()
        {
            InitializeComponent();
        }
    }
}
