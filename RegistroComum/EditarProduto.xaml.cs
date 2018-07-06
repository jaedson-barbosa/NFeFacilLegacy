using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum
{
    public sealed partial class EditarProduto : ContentDialog
    {
        public double Quantidade { get; set; }
        public double Seguro { get; set; }
        public double DespesasExtras { get; set; }
        public double ValorUnitario { get; set; }

        public EditarProduto()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
