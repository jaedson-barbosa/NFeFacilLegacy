using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesAssinatura
{
    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
