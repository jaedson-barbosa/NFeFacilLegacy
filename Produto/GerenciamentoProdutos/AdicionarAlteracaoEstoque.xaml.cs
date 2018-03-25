using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.GerenciamentoProdutos
{
    public sealed partial class AdicionarAlteracaoEstoque : ContentDialog
    {
        bool Positivo { get; set; } = true;
        double Valor { get; set; }
        internal double ValorProcessado => Valor * (Positivo ? 1 : -1);

        public AdicionarAlteracaoEstoque()
        {
            InitializeComponent();
        }
    }
}
