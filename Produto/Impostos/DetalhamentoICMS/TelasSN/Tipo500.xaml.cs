using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoICMS.TelasSN
{
    [DetalhePagina("ICMS")]
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
