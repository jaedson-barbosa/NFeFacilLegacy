using System.Xml.Serialization;

namespace BaseGeral.ModeloXML
{
    public sealed class InformacoesSuplementaresNFCe
    {
        [XmlElement("qrCode", Order = 0)]
        public string Uri { get; set; }

        [XmlElement("urlChave", Order = 1)]
        public string UriChave { get; set; }
    }
}
