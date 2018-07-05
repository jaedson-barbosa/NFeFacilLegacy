using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    public sealed partial class InfoInutilizacao : ContentDialog
    {
        public bool Homologacao { get; private set; }
        public int Serie { get; private set; }
        public int InicioNum { get; private set; }
        public int FimNum { get; private set; }
        public string Justificativa { get; private set; }

        public InfoInutilizacao()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = Justificativa.Length < 15;
        }
    }
}
