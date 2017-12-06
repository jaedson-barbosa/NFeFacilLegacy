using ServidorCertificacao.Primitivos;
using System.Security.Cryptography.X509Certificates;

namespace ServidorCertificacao.Pacotes
{
    public struct CertificadoAssinaturaDTO
    {
        public string XML { get; set; }
        public string Tag { get; set; }
        public string Id { get; set; }
        public string Serial { get; set; }
    }
}
