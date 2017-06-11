using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace BibliotecaCentral.Importacao
{
    public sealed class ImportarCertificado : Importacao
    {
        public ImportarCertificado() : base(".pfx") { }

        public async Task ImportarEAdicionarAsync(Action attLista)
        {
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            var cert = await Importar();
            if (cert != null)
            {
                using (var loja = new X509Store())
                {
                    loja.Open(OpenFlags.ReadWrite);
                    loja.Add(cert);
                }
                attLista();
            }
        }

        internal async Task<X509Certificate2> Importar()
        {
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            var arq = await abrir.PickSingleFileAsync();
            if (arq != null)
            {
                return new X509Certificate2(arq.Path, await InputTextDialogAsync("Senha do certificado"));
            }
            else
            {
                return null;
            }
        }

            public static async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox()
            {
                AcceptsReturn = false,
                Height = 32
            };
            ContentDialog dialog = new ContentDialog()
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancelar"
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }
    }
}
