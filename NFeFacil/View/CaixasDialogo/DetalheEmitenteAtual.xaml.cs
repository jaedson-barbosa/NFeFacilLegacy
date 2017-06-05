using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo
{
    public sealed partial class DetalheEmitenteAtual : ContentDialog
    {
        public bool ManipulacaoAtivada { get; set; }

        public DetalheEmitenteAtual()
        {
            this.InitializeComponent();
        }
    }
}
