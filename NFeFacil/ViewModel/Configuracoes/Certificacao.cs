using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static NFeFacil.Configuracoes.ConfiguracoesCertificacao;

namespace NFeFacil.ViewModel.Configuracoes
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

        public IEnumerable<string> Certificados => from c in _Certificados
                                                   orderby c.Subject
                                                   select c.Subject;

        public string CertificadoEscolhido
        {
            get => _Certificados.FirstOrDefault(x => x.SerialNumber == Certificado).Subject;
            set => Certificado = _Certificados.Single(x => x.Subject == value).SerialNumber;
        }
    }
}
