using BaseGeral.Log;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewCertificacao
{
    public sealed partial class SenhaMicrosoft : ContentDialog
    {
        public string Senha { get; private set; }

        public SenhaMicrosoft()
        {
            InitializeComponent();
        }

        void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(Senha))
            {
                args.Cancel = true;
                Popup.Current.Escrever(TitulosComuns.Atenção, "Por favor, infome a sua senha, sem ela não será possível acessar os seus certificados.");
            }
        }
    }
}
