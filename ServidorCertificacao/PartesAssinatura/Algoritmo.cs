using System.Xml.Serialization;

namespace ServidorCertificacao.PartesAssinatura
{
    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
