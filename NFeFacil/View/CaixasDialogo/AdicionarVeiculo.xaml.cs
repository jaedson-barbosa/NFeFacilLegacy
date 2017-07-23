using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo
{
    public sealed partial class AdicionarVeiculo : ContentDialog
    {
        public AdicionarVeiculo()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!new ValidadorVeiculo((VeiculoDI)DataContext).Validar(Popup.Current))
            {
                args.Cancel = true;
            }
        }
    }
}
