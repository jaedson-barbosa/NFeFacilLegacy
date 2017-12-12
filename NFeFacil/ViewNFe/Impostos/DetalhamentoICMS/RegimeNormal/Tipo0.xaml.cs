using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.RegimeNormal
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo0 : Page
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public Tipo0()
        {
            InitializeComponent();
        }
    }
}
