using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoICMS.TelasSN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo201 : Page
    {
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public double pICMSST { get; private set; }

        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        public Tipo201()
        {
            InitializeComponent();
        }
    }
}
