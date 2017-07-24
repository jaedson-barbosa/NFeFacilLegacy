using Comum.Primitivos;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NFeFacil.Certificacao
{
    public static class Certificados
    {
        public static async Task<ObservableCollection<CertificadoExibicao>> ObterCertificadosAsync(OrigemCertificado origem)
        {
            if (origem == OrigemCertificado.Importado)
            {
                using (var loja = new X509Store())
                {
                    loja.Open(OpenFlags.ReadOnly);
                    return (from X509Certificate2 cert in loja.Certificates
                            select new CertificadoExibicao
                            {
                                Subject = cert.Subject,
                                SerialNumber = cert.SerialNumber,
                                Local = true
                            }).GerarObs();
                }
            }
            else
            {
                var operacoes = new LAN.OperacoesServidor();
                return (await operacoes.ObterCertificados()).GerarObs();
            }
        }

        public static async Task<CertificadoAssinatura> ObterCertificadoEscolhidoAsync(string serial, OrigemCertificado origem)
        {
            if (origem == OrigemCertificado.Importado)
            {
                using (var loja = new X509Store())
                {
                    loja.Open(OpenFlags.ReadOnly);
                    var cert = loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, true)[0];
                    return new CertificadoAssinatura
                    {
                        ChavePrivada = cert.GetRSAPrivateKey(),
                        RawData = cert.RawData
                    };
                }
            }
            else
            {
                var operacoes = new LAN.OperacoesServidor();
                return await operacoes.ObterCertificado(serial);
            }
        }
    }
}
