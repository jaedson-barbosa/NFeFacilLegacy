using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.TelasRN
{
    [View.DetalhePagina("ICMS")]
    public sealed partial class Tipo51 : Page
    {
        public int modBC { get; set; }
        public double pRedBC { get; set; }
        public double pICMS { get; set; }
        public double pDif { get; set; }
        public bool Calcular { get; set; }

        public Tipo51()
        {
            InitializeComponent();
        }
    }
}
