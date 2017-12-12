using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.SimplesNacional
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo101 : Page
    {
        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        public Tipo101()
        {
            InitializeComponent();
        }
    }
}
