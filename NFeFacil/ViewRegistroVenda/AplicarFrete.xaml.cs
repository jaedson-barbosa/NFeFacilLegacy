using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class AplicarFrete : ContentDialog
    {
        internal double Valor { get; private set; }
        internal string TipoFrete { get; private set; }

        public AplicarFrete()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(TipoFrete))
            {
                args.Cancel = true;
            }
        }
    }
}
