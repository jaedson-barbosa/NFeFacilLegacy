using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Threading.Tasks;
using System.Xml;

namespace BibliotecaCentral.Certificacao
{
    public struct AssinaNFe
    {
        private NFe Nota;

        public AssinaNFe(NFe nfe)
        {
            Nota = nfe;
        }

        public async Task Assinar()
        {
            var xml = new XmlDocument();
            using (var reader = Nota.ToXElement<NFe>().CreateReader())
            {
                xml.Load(reader);
                var cert = await new Certificados().ObterCertificadoEscolhidoAsync();
                Nota.Signature = new AssinaturaXML(xml, "infNFe", Nota.Informações.Id).AssinarXML(cert);
            }
        }
    }
}
