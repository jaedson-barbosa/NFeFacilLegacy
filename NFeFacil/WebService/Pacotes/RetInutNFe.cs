using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("retInutNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct RetInutNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("infInut", Order = 0)]
        public PartesRetInutNFe.InfInut Info { get; set; }

        [XmlElement("Signature", Order = 1, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }
    }
}
