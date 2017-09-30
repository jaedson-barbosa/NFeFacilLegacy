using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoProduto
{
    public sealed partial class EscolherTipoDeclaracaoExportacao : ContentDialog
    {
        public TiposDeclaracaoExportacao TipoEscolhido { get; private set; }

        public EscolherTipoDeclaracaoExportacao()
        {
            InitializeComponent();
            cmbTipo.ItemsSource = ExtensoesPrincipal.ObterItens<TiposDeclaracaoExportacao>();
        }
    }

    public enum TiposDeclaracaoExportacao
    {
        Direta,
        Indireta
    }
}
