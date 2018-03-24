using BaseGeral.Log;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class EscolherTipoCliente : ContentDialog
    {
        public int TipoCliente { get; private set; }

        public EscolherTipoCliente()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (TipoCliente == -1)
            {
                Popup.Current.Escrever(TitulosComuns.Atenção, "Primeiro escolha uma classificação para o cliente.");
                args.Cancel = true;
            }
        }
    }
}
