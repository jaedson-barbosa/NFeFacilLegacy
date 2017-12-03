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

        public static explicit operator CertificadoAssinatura(CertificadoAssinaturaDTO dto)
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                var temp = loja.Certificates.Find(X509FindType.FindBySerialNumber, dto.Serial, true)[0];
                return new CertificadoAssinatura
                {
                    ChavePrivada = temp.GetRSAPrivateKey(),
                    RawData = temp.RawData,
                    Id = dto.Id,
                    Tag = dto.Tag,
                    XML = dto.XML
                };
            }
        }
    }
}
