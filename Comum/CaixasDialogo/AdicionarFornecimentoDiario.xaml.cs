using BaseGeral.ModeloXML.PartesDetalhes;
using Windows.UI.Xaml.Controls;

// O modelo de item da Caixa de Diálogo de Conteúdo está documentado em http://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum.CaixasDialogo
{
    public sealed partial class AdicionarFornecimentoDiario : ContentDialog
    {
        public FornecimentoDiario Contexto { get; } = new FornecimentoDiario();

        public AdicionarFornecimentoDiario()
        {
            InitializeComponent();
        }
    }
}
