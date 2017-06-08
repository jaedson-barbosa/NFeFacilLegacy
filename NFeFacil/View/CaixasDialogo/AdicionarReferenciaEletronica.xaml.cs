using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo
{
    public sealed partial class AdicionarReferenciaEletronica : ContentDialog
    {
        public string Chave { get; private set; }

        public AdicionarReferenciaEletronica()
        {
            this.InitializeComponent();
            DataContext = string.Empty;
        }
    }
}
