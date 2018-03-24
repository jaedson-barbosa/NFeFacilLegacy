using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoIPI
{
    [DetalhePagina("IPI")]
    public sealed partial class DetalharAliquota : Page
    {
        public IPI Conjunto { get; } = new IPI();
        public double Aliquota { get; private set; }

        public DetalharAliquota()
        {
            InitializeComponent();
        }
    }
}
