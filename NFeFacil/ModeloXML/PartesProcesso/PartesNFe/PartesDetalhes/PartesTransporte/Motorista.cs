using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class Motorista
    {
        public Motorista() : base() { }
        public Motorista(Motorista other)
        {
            xEnder = other.xEnder;
            xMun = other.xMun;
            UF = other.UF;
            nome = other.nome;
            inscricaoEstadual = other.inscricaoEstadual;
            CPF = other.CPF;
            CNPJ = other.CNPJ;
        }

        public string CPF { get; set; }
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string nome { get; set; }

        [XmlElement(ElementName = "IE")]
        public string inscricaoEstadual { get; set; }
        /// <summary>
        /// (Opcional)
        /// Endereço Completo.
        /// </summary>
        public string xEnder { get; set; }

        /// <summary>
        /// (Opcional)
        /// Nome do município.
        /// </summary>
        public string xMun { get; set; }

        /// <summary>
        /// (Opcional)
        /// Sigla da UF.
        /// Informar "EX" para Exterior.
        /// </summary>
        public string UF { get; set; }

        [XmlIgnore]
        public string obterDocumento
        {
            get { return CNPJ ?? CPF; }
        }

        [XmlIgnore]
        public TiposDocumento obterTipoDocumento
        {
            get
            {
                return !string.IsNullOrEmpty(CNPJ) ? TiposDocumento.CNPJ : TiposDocumento.CPF;
            }
        }
    }
}
