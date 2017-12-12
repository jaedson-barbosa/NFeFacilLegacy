using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.SimplesNacional
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo202 : Page
    {
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public string vBCST { get; private set; }
        public string pICMSST { get; private set; }
        public string vICMSST { get; private set; }

        public Tipo202()
        {
            InitializeComponent();
        }
    }
}
