using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Windows.Storage;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Certificados
    {
        private StorageFolder PastaArquivos = ApplicationData.Current.LocalFolder;

        public async Task<IEnumerable<X509Certificate2>> ObterRegistroAsync(FonteCertificacao fonte)
        {
            switch (fonte)
            {
                case FonteCertificacao.PastaApp:
                    return from arq in await PastaArquivos.GetFilesAsync()
                           where arq.FileType == ".pfx"
                           select new X509Certificate2(arq.Path);
                case FonteCertificacao.RepositorioWindows:
                    var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    loja.Open(OpenFlags.ReadOnly);
                    return loja.Certificates.Cast<X509Certificate2>();
                default:
                    throw new Exception("Fonte de certificação desconhecida.");
            }
        }
    }
}
