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
            var repo = new Repositorio.Certificados();
            if (Nota.Signature?.HasChildNodes ?? false)
            {
                throw new System.Exception("A NFe já está assinada");
            }
            else if (string.IsNullOrEmpty(repo.Escolhido))
            {
                throw new System.Exception("Nenhum certificado padrão selecionado.");
            }
            var xml = new XmlDocument();
            xml.Load(Nota.ToXElement<NFe>().CreateReader());
            var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            loja.Open(OpenFlags.ReadOnly);
            var certs = loja.Certificates.Find(X509FindType.FindBySerialNumber, repo.Escolhido, true);
            Nota.Signature = AssinaXML.AssinarXML(xml, certs[0]);
        }
    }
}
