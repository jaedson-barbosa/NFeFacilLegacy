using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Emitente
    {
        public Emitente() : base() { }
        public Emitente(Emitente other)
        {
            CNPJ = other.CNPJ;
            inscricaoEstadual = other.inscricaoEstadual;
            nome = other.nome;
            nomeFantasia = other.nomeFantasia;
            endereco = new enderecoCompleto(other.endereco);
            regimeTributario = other.regimeTributario;
            IEST = other.IEST;
            IM = other.IM;
            CNAE = other.CNAE;
        }
        [Key]
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
