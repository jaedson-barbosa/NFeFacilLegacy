using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML
{
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class Processo
    {
        [XmlAttribute]
        public string versao = "3.10";
        public NFe NFe { get; set; }
        [XmlElement("protNFe")]
        public ProtocoloNFe ProtNFe { get; set; }
    }
}
