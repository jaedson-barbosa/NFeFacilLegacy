using BaseGeral.View;
using Windows.UI.Xaml.Controls;

namespace Venda.Impostos.DetalhamentoICMS.TelasSN
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
