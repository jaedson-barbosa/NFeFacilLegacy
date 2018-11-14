using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.RelatorioProduto01
{
    public sealed partial class InfoTabela : UserControl
    {
        string Categoria, Fornecedor;

        public InfoTabela(string categoria, string fornecedor)
        {
            InitializeComponent();
            Categoria = categoria;
            Fornecedor = fornecedor;
        }
    }
}
