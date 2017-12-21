using NFeFacil.Certificacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso
{
    public sealed class ProtocoloNFe : ISignature
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; } = "3.10";

        [XmlElement("infProt", Order = 0)]
        public InfoProtocolo InfProt { get; set; }

        [XmlElement("Signature", Order = 1, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }
    }
}
