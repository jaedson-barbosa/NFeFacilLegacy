using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BibliotecaCentral.Certificacao
{
    public static class Certificados
    {
        public async static Task<ObservableCollection<CertificadoFundamental>> ObterRegistroRepositorioAsync()
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                IEnumerable<CertificadoFundamental> certs;
                if (string.IsNullOrEmpty(ConfiguracoesCertificacao.IPServidorCertificacao))
                {
                    certs = from X509Certificate2 cert in loja.Certificates
                            select new CertificadoFundamental(cert.Subject, cert.SerialNumber);
                }
                else
                {
                    certs = await ServidorCertificacao.ObterCertificados();
                }
                return new ObservableCollection<CertificadoFundamental>(certs);
            }
        }

        public static X509Certificate2 ObterCertificadoEscolhido()
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                return loja.Certificates.Find(X509FindType.FindBySerialNumber, ConfiguracoesCertificacao.CertificadoEscolhido, true)[0];
            }
        }
    }
}
