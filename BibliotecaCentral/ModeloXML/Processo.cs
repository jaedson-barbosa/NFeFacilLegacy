using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML
{
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class Processo
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; } = "3.10";
        [XmlElement(nameof(NFe), Order = 0)]
        public NFe NFe { get; set; }
        [XmlElement("protNFe", Order = 1)]
        public ProtocoloNFe ProtNFe { get; set; }
    }
}
