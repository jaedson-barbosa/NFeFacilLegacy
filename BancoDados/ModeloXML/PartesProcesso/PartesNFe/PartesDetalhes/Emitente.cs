using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Emitente
    {
        [XmlElement(Order = 0)]
        public long CNPJ { get; set; }

        [XmlElement(ElementName = "xNome", Order = 1)]
        public string Nome { get; set; }

        [XmlElement(ElementName = "xFant", Order = 2)]
        public string NomeFantasia { get; set; }

        [XmlElement(ElementName = "enderEmit", Order = 3)]
        public EnderecoCompleto Endereco { get; set; } = new EnderecoCompleto();

        [XmlElement(ElementName = "IE", Order = 4)]
        public long InscricaoEstadual { get; set; }

        [XmlElement(Order = 5)]
        public string IEST { get; set; }

        [XmlElement(Order = 6)]
        public string IM { get; set; }

        [XmlElement(Order = 7)]
        public string CNAE { get; set; }

        [XmlElement(ElementName = "CRT", Order = 8)]
        public int RegimeTributario { get; set; }
    }
}
