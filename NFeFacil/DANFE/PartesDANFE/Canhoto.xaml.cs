using NFeFacil.DANFE.Pacotes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class Canhoto : UserControl
    {
        public DadosCabecalho Contexto { get; set; }

        DimensoesPadrao Dimensoes { get; } = new DimensoesPadrao();
        GridLength ColunaMeio => CMToLength(10);

        public Canhoto()
        {
            InitializeComponent();
        }
    }
}
