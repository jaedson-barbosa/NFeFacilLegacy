using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    public sealed partial class AdicionarAlteracaoEstoque : ContentDialog
    {
        public double Valor { get; set; }

        public AdicionarAlteracaoEstoque()
        {
            InitializeComponent();
        }
    }
}
