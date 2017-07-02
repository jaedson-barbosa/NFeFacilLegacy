using BibliotecaCentral.Certificacao;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
using System.Xml;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso
{
    [XmlRoot(nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class NFe : ISignature
    {
        [XmlElement("infNFe")]
        public Detalhes Informações { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }
    }

    public sealed class Assinatura
    {
        public SignedInfo SignedInfo { get; set; }
        public string SignatureValue { get; set; }
        public DetalhesChave KeyInfo { get; set; }
    }

    public struct DetalhesChave
    {
        public DadosChave X509Data { get; set; }
    }

    public struct DadosChave
    {
        public string X509Certificate { get; set; }
    }

    public struct SignedInfo
    {
        public Algoritmo CanonicalizationMethod { get; set; }
        public Algoritmo SignatureMethod { get; set; }

        public Referencia Reference { get; set; }
    }

    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }

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
