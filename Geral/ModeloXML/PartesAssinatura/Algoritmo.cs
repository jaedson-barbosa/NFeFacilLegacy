using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesAssinatura
{
    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
