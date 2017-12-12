using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.RegimeNormal
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo20 : Page
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public Tipo20()
        {
            InitializeComponent();
        }
    }
}
