using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.ProdutoEspecial
{
    public sealed partial class DefinirPapel : ContentDialog
    {
        public string NRECOPI { get; private set; }

        public DefinirPapel(BaseGeral.ModeloXML.IProdutoEspecial prod)
        {
            InitializeComponent();
            NRECOPI = prod?.NRECOPI;
        }
    }
}
