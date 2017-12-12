using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo40_41_50 : Page
    {
        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public Tipo40_41_50()
        {
            InitializeComponent();
        }
    }
}
