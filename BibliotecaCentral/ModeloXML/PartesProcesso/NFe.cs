using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
using System.Xml;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso
{
    [XmlRoot(nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class NFe
    {
        [XmlElement("infNFe")]
        public Detalhes Informações { get; set; }

        [XmlAnyElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public XmlElement Signature { get; set; }
    }
}
