using System;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Emitente
    {
        [XmlIgnore]
        public Guid Id { get; set; }

        [XmlIgnore]
        public DateTime UltimaData { get; set; }

        public string CNPJ { get; set; }
        [XmlElement(ElementName = "xNome")]
        public string nome { get; set; }
        [XmlElement(ElementName = "xFant")]
        public string nomeFantasia { get; set; }

        [XmlElement(ElementName = "enderEmit")]
        public enderecoCompleto endereco { get; set; } = new enderecoCompleto();

        [XmlElement(ElementName = "IE")]
        public string inscricaoEstadual { get; set; }
        public string IEST { get; set; }
        public string IM { get; set; }
        public string CNAE { get; set; }
        [XmlElement(ElementName = "CRT")]
        public int regimeTributario { get; set; }
    }
}
