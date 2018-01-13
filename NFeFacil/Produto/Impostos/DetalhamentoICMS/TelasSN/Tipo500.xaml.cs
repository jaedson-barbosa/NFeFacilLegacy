using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.TelasSN
{
    [View.DetalhePagina("ICMS")]
    public sealed partial class Tipo500 : Page
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }

        public Tipo500()
        {
            InitializeComponent();
        }
    }
}
