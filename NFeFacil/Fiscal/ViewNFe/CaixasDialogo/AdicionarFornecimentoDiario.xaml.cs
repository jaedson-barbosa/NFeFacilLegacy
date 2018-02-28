using NFeFacil.ModeloXML.PartesDetalhes;
using Windows.UI.Xaml.Controls;

// O modelo de item da Caixa de Diálogo de Conteúdo está documentado em http://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFe.CaixasDialogo
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
