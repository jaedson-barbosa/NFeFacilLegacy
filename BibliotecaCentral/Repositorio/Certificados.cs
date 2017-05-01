using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Windows.Storage;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Certificados
    {
        private ApplicationDataContainer pasta = ApplicationData.Current.LocalSettings;

        public IEnumerable<X509Certificate2> Registro
        {
            get
            {
                var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                loja.Open(OpenFlags.ReadOnly);
                return loja.Certificates.Cast<X509Certificate2>();
            }
        }

        public string Escolhido
        {
            get { return pasta.Values[nameof(Escolhido)] as string; }
            set { pasta.Values[nameof(Escolhido)] = value; }
        }
    }
}
