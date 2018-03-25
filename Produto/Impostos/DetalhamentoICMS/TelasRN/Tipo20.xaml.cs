using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo20 : Page
    {
        public int modBC { get; set; }
        public double pRedBC { get; set; }
        public double pICMS { get; set; }
        public string motDesICMS { get; set; }

        public Tipo20()
        {
            InitializeComponent();
        }
    }
}
