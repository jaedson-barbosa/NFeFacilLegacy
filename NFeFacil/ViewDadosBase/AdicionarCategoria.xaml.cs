using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class AdicionarCategoria : ContentDialog
    {
        public string Nome { get; set; }

        public AdicionarCategoria(string nome = "")
        {
            InitializeComponent();
            Nome = nome;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = Nome.Length < 3;
        }
    }
}
