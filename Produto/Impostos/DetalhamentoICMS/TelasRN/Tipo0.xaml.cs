using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.TelasRN
{
    [View.DetalhePagina("ICMS")]
    public sealed partial class Tipo0 : Page
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public Tipo0()
        {
            InitializeComponent();
        }
    }
}
