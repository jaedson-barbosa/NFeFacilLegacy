using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.RegimeNormal
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo51 : Page
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSOp { get; set; }
        public string pDif { get; set; }
        public string vICMSDif { get; set; }

        public Tipo51()
        {
            InitializeComponent();
        }
    }
}
