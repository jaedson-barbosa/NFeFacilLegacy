using System.Security.Cryptography.X509Certificates;

namespace BibliotecaCentral.ItensBD
{
    public sealed class Certificado
    {
        public string NumeroSerial { get; set; }
        public byte[] Data { get; set; }
        public string Nome { get; set; }

        internal X509Certificate2 Obter()
        {
            return new X509Certificate2(Data);
        }
    }
}
