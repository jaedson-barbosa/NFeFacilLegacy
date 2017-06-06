using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Destinatario
    {
        [XmlElement(Order = 0)]
        public string CPF { get; set; }
        [XmlElement(Order = 1)]
        public string CNPJ { get; set; }
        [XmlElement(Order = 2)]
        public string idEstrangeiro { get; set; }

        [XmlElement(ElementName = "xNome", Order = 3)]
        public string nome { get; set; }

        [XmlElement(ElementName = "enderDest", Order = 4)]
        public enderecoCompleto endereco { get; set; } = new enderecoCompleto();

        [XmlElement(ElementName = "indIEDest", Order = 5)]
        public int indicadorIE { get; set; } = 9;

        [XmlElement(ElementName = "IE", Order = 6)]
        public string inscricaoEstadual { get; set; }

        [XmlElement(Order = 7)]
        public string ISUF { get; set; }

        [XmlElement(Order = 8)]
        public string email { get; set; }

        [XmlIgnore]
        public string Documento => idEstrangeiro ?? CNPJ ?? CPF;
        [XmlIgnore]
        public TiposDocumento obterTipoDocumento => (!string.IsNullOrEmpty(idEstrangeiro)) ? TiposDocumento.idEstrangeiro :
            (!string.IsNullOrEmpty(CNPJ)) ? TiposDocumento.CNPJ : TiposDocumento.CPF;
    }
}
