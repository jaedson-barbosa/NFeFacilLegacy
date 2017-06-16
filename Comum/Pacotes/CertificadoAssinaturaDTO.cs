using Comum.Primitivos;
using System.Security.Cryptography;

namespace Comum.Pacotes
{
    public struct CertificadoAssinaturaDTO
    {
        public RSAParameters ParametrosChavePrivada { get; set; }
        public byte[] RawData { get; set; }

        public static explicit operator CertificadoAssinatura(CertificadoAssinaturaDTO dto)
        {
            var chave = RSA.Create();
            chave.ImportParameters(dto.ParametrosChavePrivada);
            return new CertificadoAssinatura
            {
                ChavePrivada = chave,
                RawData = dto.RawData
            };
        }
    }
}
