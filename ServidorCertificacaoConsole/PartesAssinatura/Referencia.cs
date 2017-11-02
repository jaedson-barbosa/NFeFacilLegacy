using System.Xml.Serialization;

namespace ServidorCertificacaoConsole.PartesAssinatura
{
    public struct Referencia
    {
        [XmlAttribute]
        public string URI { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Transform")]
        public Algoritmo[] Transforms { get; set; }

        public Algoritmo DigestMethod { get; set; }
        public string DigestValue { get; set; }
    }
}
