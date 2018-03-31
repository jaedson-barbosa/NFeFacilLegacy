using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoIPI
{
    [DetalhePagina("IPI")]
    public sealed partial class DetalharSimples : Page
    {
        public IPI Conjunto { get; } = new IPI()
        {
            Corpo = new IPINT()
        };

        public DetalharSimples()
        {
            InitializeComponent();
        }
    }
}
