using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BibliotecaCentral.Certificacao
{
    public sealed class Certificados
    {
        public ObservableCollection<X509Certificate2> ObterRegistroRepositorio()
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                return new ObservableCollection<X509Certificate2>(loja.Certificates.Cast<X509Certificate2>());
            }
        }

        public X509Certificate2 ObterCertificadoEscolhido()
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                return loja.Certificates.Find(X509FindType.FindBySerialNumber, new ConfiguracoesCertificacao().CertificadoEscolhido, true)[0];
            }
        }
    }
}
