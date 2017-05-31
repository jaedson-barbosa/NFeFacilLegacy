using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Emitente
    {
        [XmlElement(Order = 0)]
        public string CNPJ { get; set; }
        [XmlElement(ElementName = "xNome", Order = 1)]
        public string nome { get; set; }
        [XmlElement(ElementName = "xFant", Order = 2)]
        public string nomeFantasia { get; set; }

        [XmlElement(ElementName = "enderEmit", Order = 3)]
        public enderecoCompleto endereco { get; set; } = new enderecoCompleto();

        [XmlElement(ElementName = "IE", Order = 4)]
        public string inscricaoEstadual { get; set; }
        [XmlElement(Order = 5)]
        public string IEST { get; set; }
        [XmlElement(Order = 6)]
        public string IM { get; set; }
        [XmlElement(Order = 7)]
        public string CNAE { get; set; }
        [XmlElement(ElementName = "CRT", Order = 8)]
        public int regimeTributario { get; set; }
    }
}
