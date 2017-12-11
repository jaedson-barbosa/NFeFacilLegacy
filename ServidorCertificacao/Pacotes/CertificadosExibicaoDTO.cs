using System.Security.Cryptography.X509Certificates;

namespace ServidorCertificacao.Pacotes
{
    public sealed class CertificadosExibicaoDTO
    {
        public CertificadoExibicao[] Registro { get; set; }

        public CertificadosExibicaoDTO()
        {
            var Loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            Loja.Open(OpenFlags.ReadOnly);
            var quant = Loja.Certificates.Count;
            Registro = new CertificadoExibicao[quant];
            for (int i = 0; i < quant; i++)
            {
                var item = Loja.Certificates[i];
                Registro[i] = new CertificadoExibicao
                {
                    SerialNumber = item.SerialNumber,
                    Subject = item.Subject
                };
            }
        }

        public struct CertificadoExibicao
        {
            public string Subject { get; set; }
            public string SerialNumber { get; set; }
            public bool Local { get; set; }
        }
    }
}
