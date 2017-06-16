using System.Security.Cryptography;

namespace Comum.Primitivos
{
    public struct CertificadoAssinatura
    {
        public RSA ChavePrivada { get; set; }
        public byte[] RawData { get; set; }
    }
}
