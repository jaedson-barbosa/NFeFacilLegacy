using BibliotecaCentral.ItensBD;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BibliotecaCentral.Certificacao
{
    public sealed class Certificados
    {
        public IEnumerable<X509Certificate2> ObterRegistroRepositorio()
        {
            var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            loja.Open(OpenFlags.ReadOnly);
            return loja.Certificates.Cast<X509Certificate2>();
        }

        public IEnumerable<Certificado> ObterRegistroBanco()
        {
            using (var contexto = new AplicativoContext())
            {
                return contexto.Certificados;
            }
        }

        public X509Certificate2 ObterCertificadoEscolhido()
        {
            var config = new ConfiguracoesCertificacao();
            if (config.FonteEscolhida == FonteCertificacao.RepositorioWindows)
            {
                var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                loja.Open(OpenFlags.ReadOnly);
                return loja.Certificates.Find(X509FindType.FindBySerialNumber, config.CertificadoEscolhido, true)[0];
            }
            else
            {
                using (var contexto = new AplicativoContext())
                {
                    var id = config.CertificadoEscolhido;
                    return new X509Certificate2(contexto.Certificados.Find(id).Data);
                }
            }
        }
    }
}
