using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo60 : Page
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }

        public Tipo60()
        {
            InitializeComponent();
        }
    }
}
