using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoICMSUFDest
{
    [DetalhePagina("ICMS para a UF de destino")]
    public sealed partial class Detalhar : Page, IDadosICMSUFDest
    {
        public ICMSUFDest Imposto { get; } = new ICMSUFDest();

        public Detalhar()
        {
            InitializeComponent();
        }
    }
}
