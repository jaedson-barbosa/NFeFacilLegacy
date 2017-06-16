using Comum.Primitivos;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BibliotecaCentral.Certificacao
{
    public class Certificados
    {
        public async Task<ObservableCollection<CertificadoExibicao>> ObterCertificadosAsync()
        {
            if (Atual == Origem.Local)
            {
                using (var loja = new X509Store())
                {
                    loja.Open(OpenFlags.ReadOnly);
                    return (from X509Certificate2 cert in loja.Certificates
                            select new CertificadoExibicao
                            {
                                Subject = cert.Subject,
                                SerialNumber = cert.SerialNumber
                            }).GerarObs();
                }
            }
            else
            {
                var operacoes = new LAN.OperacoesServidor(ConfiguracoesCertificacao.IPServidorCertificacao);
                return (await operacoes.ObterCertificados()).GerarObs();
            }
        }

        public async Task<CertificadoAssinatura> ObterCertificadoEscolhidoAsync()
        {
            var serial = ConfiguracoesCertificacao.CertificadoEscolhido;
            if (Atual == Origem.Local)
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
                var operacoes = new LAN.OperacoesServidor(ConfiguracoesCertificacao.IPServidorCertificacao);
                return await operacoes.ObterCertificado(serial);
            }
        }

        Origem Atual => string.IsNullOrEmpty(ConfiguracoesCertificacao.IPServidorCertificacao) ? Origem.Local : Origem.LAN;

        private enum Origem
        {
            Local,
            LAN
        }
    }
}
