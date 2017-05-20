using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Destinatario
    {
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
        public string inscricaoEstadual { get; set; }
        public string ISUF { get; set; }
        public string email { get; set; }

        [XmlIgnore]
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
