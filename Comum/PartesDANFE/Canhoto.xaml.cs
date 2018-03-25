using Comum.PacotesDANFE;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static BaseGeral.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Comum.PartesDANFE
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
