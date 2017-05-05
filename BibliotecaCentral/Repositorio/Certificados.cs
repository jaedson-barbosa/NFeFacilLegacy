using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Certificados
    {
        private StorageFolder PastaArquivos = ApplicationData.Current.LocalFolder;

        public IEnumerable<X509Certificate2> ObterRegistroRepositorio()
        {
            var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            loja.Open(OpenFlags.ReadOnly);
            return loja.Certificates.Cast<X509Certificate2>();
        }

        public async Task<IEnumerable<string>> ObterRegistroPastaAsync()
        {
            return from arq in await PastaArquivos.GetFilesAsync()
                   where arq.FileType == ".pfx"
                   select arq.DisplayName;
        }

        public async Task<X509Certificate2> ObterCertificadoEscolhidoAsync()
        {
            var config = new Configuracoes.ConfiguracoesCertificacao();
            if (config.FonteEscolhida == FonteCertificacao.RepositorioWindows)
            {
                var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                loja.Open(OpenFlags.ReadOnly);
                return loja.Certificates.Find(X509FindType.FindBySerialNumber, config.CertificadoEscolhido, true)[0];
            }
            else
            {
                var arq = await PastaArquivos.GetFileAsync(config.CertificadoEscolhido+".pfx");
                return new X509Certificate2(arq.Path, await InputTextDialogAsync("Senha do certificado"));
            }
        }

        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox()
            {
                AcceptsReturn = false,
                Height = 32
            };
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = false;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancelar";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }
    }
}
