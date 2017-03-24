using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static NFeFacil.Configuracoes.ConfiguracoesCertificacao;

namespace NFeFacil.ViewModel.Configurações
{
    public sealed class Certificacao
    {
        private IEnumerable<X509Certificate2> _Certificados
        {
            get
            {
                var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                loja.Open(OpenFlags.ReadOnly);
                return from X509Certificate2 cert in loja.Certificates
                       select cert;
            }
        }

        public IEnumerable<string> Certificados
        {
            get
            {
                var subjects = from c in _Certificados
                               select c.Subject;
                return subjects.Distinct().OrderBy(x => x);
            }
        }

        public string CertificadoEscolhido
        {
            get
            {
                try
                {
                    return _Certificados.Single(x => x.SerialNumber == Certificado).Subject;
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
            set { Certificado = _Certificados.Single(x => x.Subject == value).SerialNumber; }
        }
    }
}
