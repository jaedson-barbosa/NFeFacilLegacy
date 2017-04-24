using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso
{
    [XmlRoot(nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class NFe
    {
        [XmlElement("infNFe")]
        public Detalhes Informações { get; set; }

        [XmlAnyElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public XElement Signature { get; set; }
    }
}
