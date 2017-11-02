using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.Certificacao
{
    public sealed class ImportarCertificado
    {
        public async Task<bool> ImportarEAdicionarAsync()
        {
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            abrir.FileTypeFilter.Add(".cer");
            var cert = await Importar();
            if (cert != null)
            {
                using (var loja = new X509Store())
                {
                    loja.Open(OpenFlags.ReadWrite);
                    loja.Add(cert);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        async Task<X509Certificate2> Importar()
        {
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            var arq = await abrir.PickSingleFileAsync();
            if (arq != null)
            {
                var pasta = ApplicationData.Current.TemporaryFolder;
                var novoArq = await arq.CopyAsync(pasta, arq.Name, NameCollisionOption.ReplaceExisting);
                var entrada = new DefinirSenhaCertificado();
                if (await entrada.ShowAsync() == ContentDialogResult.Primary)
                {
                    var senha = entrada.Senha;
                    if (string.IsNullOrEmpty(senha))
                    {
                        return new X509Certificate2(novoArq.Path);
                    }
                    else
                    {
                        return new X509Certificate2(novoArq.Path, senha, X509KeyStorageFlags.PersistKeySet);
                    }
                }
            }
            return null;
        }
    }
}
