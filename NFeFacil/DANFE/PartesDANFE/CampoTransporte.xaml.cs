using NFeFacil.DANFE.Pacotes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class CampoTransporte : UserControl
    {
        public DadosMotorista Contexto { get; set; }

        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();

        GridLength Coluna00 => CentimeterToLength(6.5);
        GridLength Coluna01 => CentimeterToLength(3);
        GridLength Coluna02 => CentimeterToLength(2.5);
        GridLength Coluna03 => CentimeterToLength(2.5);

        GridLength Coluna10 => CentimeterToLength(8.5);
        GridLength Coluna11 => CentimeterToLength(6);

        GridLength Coluna4 => CentimeterToLength(1);
        GridLength Coluna5 => CentimeterToLength(3.5);

        public CampoTransporte()
        {
            InitializeComponent();
        }
    }
}
