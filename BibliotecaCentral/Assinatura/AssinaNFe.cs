using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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

        public async Task AssinarAsync()
        {
            if (Nota.Signature?.HasChildNodes ?? false)
            {
                throw new System.Exception("A NFe já está assinada");
            }
            var xml = new XmlDocument();
            xml.Load(Nota.ToXElement<NFe>().CreateReader());
            var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            loja.Open(OpenFlags.ReadOnly);
            var cert = await new Repositorio.Certificados().ObterCertificadoEscolhidoAsync();
            Nota.Signature = AssinaXML.AssinarXML(xml, cert);
        }
    }
}
