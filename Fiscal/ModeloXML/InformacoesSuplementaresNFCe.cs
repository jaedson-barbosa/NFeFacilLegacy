using System.Xml;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML
{
    public sealed class InformacoesSuplementaresNFCe
    {
        [XmlElement("qrCode", Order = 0)]
        public XmlCDataSection QR
        {
            get
            {
                var doc = new XmlDocument();
                return doc.CreateCDataSection(Uri);
            }
            set => Uri = value.Value;
        }

        [XmlIgnore]
        public string Uri { get; set; }
    }
}
