using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Emitente
    {
        public Emitente() : base() { }
        public Emitente(Emitente other)
        {
            nomeFantasia = other.nomeFantasia;
            endereço = new EnderecoCompleto(other.endereço);
            regimeTributario = other.regimeTributario;
            IEST = other.IEST;
            IM = other.IM;
            CNAE = other.CNAE;
        }
        public string CNPJ { get; set; }
        [XmlElement(ElementName = "xNome")]
        public string nome { get; set; }
        [XmlElement(ElementName = "xFant")]
        public string nomeFantasia { get; set; }

        [XmlElement(ElementName = "enderEmit")]
        public EnderecoCompleto endereço { get; set; } = new EnderecoCompleto();

        [XmlElement(ElementName = "IE")]
        public string inscricaoEstadual { get; set; }
        public string IEST { get; set; }
        public string IM { get; set; }
        public string CNAE { get; set; }
        [XmlElement(ElementName = "CRT")]
        public int regimeTributario { get; set; }
    }
}
