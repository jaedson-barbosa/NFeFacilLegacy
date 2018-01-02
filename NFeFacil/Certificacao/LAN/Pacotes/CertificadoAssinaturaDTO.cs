using NFeFacil.Certificacao.LAN.Primitivos;
using System.Security.Cryptography.X509Certificates;

namespace NFeFacil.Certificacao.LAN.Pacotes
{
    public struct CertificadoAssinaturaDTO
    {
        public string XML { get; set; }
        public string Tag { get; set; }
        public string Id { get; set; }
        public string Serial { get; set; }
    }
}
