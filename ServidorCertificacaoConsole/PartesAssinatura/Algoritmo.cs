using System.Xml.Serialization;

namespace ServidorCertificacaoConsole.PartesAssinatura
{
    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
