using System.Xml.Serialization;

namespace NFeFacil.ModeloXML
{
    public sealed class InformacoesSuplementaresNFCe
    {
        [XmlElement("qrCode", Order = 0)]
        public string QR
        {
            get => $"<![CDATA[{Uri}]]>";
            set => Uri = value.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty);
        }

        [XmlIgnore]
        public string Uri { get; set; }
    }
}
