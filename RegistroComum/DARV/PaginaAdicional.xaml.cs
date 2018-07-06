using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.DARV
{
    public sealed partial class PaginaAdicional : UserControl
    {
        public PaginaAdicional(ProdutosDARV produtos)
        {
            InitializeComponent();
            produtosPagAd.Content = produtos;
        }
    }
}
