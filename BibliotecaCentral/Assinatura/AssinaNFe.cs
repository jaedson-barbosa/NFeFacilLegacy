using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

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
                //throw new System.Exception("A NFe já está assinada");
                return;
            }
            var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            loja.Open(OpenFlags.ReadOnly);
            var repo = new Repositorio.Certificados();
            var certs = loja.Certificates.Find(X509FindType.FindBySerialNumber, repo.Escolhido, true);
            var xml = new XmlDocument();
            xml.Load(Nota.ToXElement<NFe>().CreateReader());
            Nota.Signature = AssinaXML.AssinarXML(xml, certs[0]);
        }
    }
}
