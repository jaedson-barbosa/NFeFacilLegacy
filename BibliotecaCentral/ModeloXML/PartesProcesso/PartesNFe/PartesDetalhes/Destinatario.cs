using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Destinatario
    {
        public Destinatario() : base() { }
        public Destinatario(Destinatario other)
        {
            endereco = new enderecoCompleto(other.endereco);
            indicadorIE = (ushort)other.indicadorIE;
            ISUF = other.ISUF;
            email = other.email;
            nome = other.nome;
            inscricaoEstadual = other.inscricaoEstadual;
            CPF = other.CPF;
            CNPJ = other.CNPJ;
            idEstrangeiro = other.idEstrangeiro;
        }

        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string idEstrangeiro { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string nome { get; set; }

        [XmlElement(ElementName = "enderDest")]
        public enderecoCompleto endereco { get; set; } = new enderecoCompleto();

        [XmlElement(ElementName = "indIEDest")]
        public int indicadorIE { get; set; } = 9;

        [XmlElement(ElementName = "IE")]
        public string inscricaoEstadual { get; set; } = string.Empty;
        public string ISUF { get; set; }
        public string email { get; set; }

        [XmlIgnore]
        [Key]
        public string Documento
        {
            get => idEstrangeiro ?? CNPJ ?? CPF;
            set
            {
                if (!string.IsNullOrEmpty(idEstrangeiro))
                {
                    idEstrangeiro = value;
                }
                else if (!string.IsNullOrEmpty(CNPJ))
                {
                    CNPJ = value;
                }
                else
                {
                    CPF = value;
                }
            }
        }
        [XmlIgnore]
        public TiposDocumento obterTipoDocumento => (!string.IsNullOrEmpty(idEstrangeiro)) ? TiposDocumento.idEstrangeiro :
            (!string.IsNullOrEmpty(CNPJ)) ? TiposDocumento.CNPJ : TiposDocumento.CPF;
    }
}
