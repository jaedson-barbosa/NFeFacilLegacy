using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class AdicionarVeiculo : ContentDialog
    {
        public AdicionarVeiculo()
        {
            InitializeComponent();
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
