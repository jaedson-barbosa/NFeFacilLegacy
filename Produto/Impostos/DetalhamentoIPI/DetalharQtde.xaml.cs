using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoIPI
{
    [DetalhePagina("IPI")]
    public sealed partial class DetalharQtde : Page
    {
        public IPI Conjunto { get; } = new IPI();
        public double ValorUnitario { get; private set; }

        public DetalharQtde()
        {
            InitializeComponent();
        }
    }
}
