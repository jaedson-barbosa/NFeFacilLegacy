using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoII
{
    [DetalhePagina("Imposto de importação")]
    public sealed partial class Detalhar : Page, IDadosII
    {
        public II Imposto { get; } = new II();

        public Detalhar()
        {
            InitializeComponent();
        }
    }
}
