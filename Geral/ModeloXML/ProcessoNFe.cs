using System.Xml.Serialization;

namespace BaseGeral.ModeloXML
{
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class ProcessoNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; } = "3.10";
        [XmlElement(nameof(NFe), Order = 0)]
        public NFe NFe { get; set; }
        [XmlElement("protNFe", Order = 1)]
        public ProtocoloNFe ProtNFe { get; set; }
    }
}
