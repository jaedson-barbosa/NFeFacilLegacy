using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Security.Cryptography.X509Certificates;

namespace BibliotecaCentral.Assinatura
{
    public sealed class AssinaNFe
    {
        private NFe Nota;
        public AssinaNFe(NFe nfe)
        {
            Nota = nfe;
        }

        public void Assinar()
        {
            if (Nota.Signature?.HasChildNodes ?? false)
            {
                var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                loja.Open(OpenFlags.ReadOnly);
                var certs = loja.Certificates.Find(X509FindType.FindBySerialNumber, Configuracoes.ConfiguracoesCertificacao.Certificado, true);
                Nota.Signature = AssinaXML.AssinarXML(Nota.ToXmlElement(Nota.GetType()), certs[0]);
            }
        }
    }
}
