using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class TipoICMSST : Page
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }
        public string vBCSTDest { get; set; }
        public string vICMSSTDest { get; set; }

        public TipoICMSST()
        {
            InitializeComponent();
        }
    }
}
