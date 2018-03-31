using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.CaixasDialogoProduto
{
    public sealed partial class EscolherTipoDeclaracaoExportacao : ContentDialog
    {
        int TipoEscolhido { get; set; }
        public bool Direta => TipoEscolhido == 0;

        public EscolherTipoDeclaracaoExportacao()
        {
            InitializeComponent();
        }
    }
}
