using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura
{
    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
