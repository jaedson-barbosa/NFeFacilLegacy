using BaseGeral.Certificacao.LAN.Primitivos;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BaseGeral.Certificacao
{
    public static class Certificados
    {
        public static async Task<IEnumerable<CertificadoExibicao>> ObterCertificadosAsync(OrigemCertificado origem)
        {
            if (origem == OrigemCertificado.Importado)
            {
                using (var loja = new X509Store())
                {
                    loja.Open(OpenFlags.ReadOnly);
                    return from X509Certificate2 cert in loja.Certificates
                           select new CertificadoExibicao
                           {
                               Subject = cert.Subject,
                               SerialNumber = cert.SerialNumber,
                           };
                }
            }
            else
            {
                var operacoes = new LAN.OperacoesServidor();
                return await operacoes.ObterCertificados();
            }
        }
    }
}
