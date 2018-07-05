using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum.CaixasDialogo
{
    public sealed partial class AdicionarReferenciaEletronica : ContentDialog
    {
        public string Chave { get; private set; }

        public AdicionarReferenciaEletronica()
        {
            InitializeComponent();
            DataContext = string.Empty;
        }
    }
}
