using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.TelasRN
{
    [View.DetalhePagina("ICMS")]
    public sealed partial class Tipo70 : Page
    {
        public int modBC { get; set; }
        public double pRedBC { get; set; }
        public double pICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public string motDesICMS { get; set; }

        public Tipo70()
        {
            InitializeComponent();
        }
    }
}
