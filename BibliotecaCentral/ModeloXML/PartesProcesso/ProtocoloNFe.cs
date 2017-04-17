using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso
{
    public sealed class ProtocoloNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; } = "3.10";

        [XmlElement("infProt")]
        public InfoProtocolo InfProt { get; set; }
    }
}
