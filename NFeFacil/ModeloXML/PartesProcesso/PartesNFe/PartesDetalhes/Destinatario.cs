using PropertyChanged;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    [ImplementPropertyChanged]
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
        public int indicadorIE { get; set; } = 1;

        [XmlElement(ElementName = "IE")]
        public string inscricaoEstadual { get; set; }
        public string ISUF { get; set; }
        public string email { get; set; }

        [XmlIgnore]
        public string obterDocumento => (!string.IsNullOrEmpty(idEstrangeiro)) ? idEstrangeiro : (!string.IsNullOrEmpty(CNPJ)) ? CNPJ : CPF;
        [XmlIgnore]
        public TiposDocumento obterTipoDocumento => (!string.IsNullOrEmpty(idEstrangeiro)) ? TiposDocumento.idEstrangeiro :
            (!string.IsNullOrEmpty(CNPJ)) ? TiposDocumento.CNPJ : TiposDocumento.CPF;
    }
}
