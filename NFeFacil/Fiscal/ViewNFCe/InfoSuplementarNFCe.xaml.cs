using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFCe
{
    public sealed partial class InfoSuplementarNFCe : ContentDialog
    {
        public string IdToken { get; private set; }
        public string CSC { get; private set; }

        public InfoSuplementarNFCe()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = string.IsNullOrEmpty(IdToken) || string.IsNullOrEmpty(CSC);
        }
    }
}
