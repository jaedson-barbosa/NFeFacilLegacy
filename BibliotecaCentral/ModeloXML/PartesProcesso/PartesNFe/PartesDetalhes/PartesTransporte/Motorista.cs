using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class Motorista
    {
        public Motorista() : base() { }
        public Motorista(Motorista other)
        {
            XEnder = other.XEnder;
            XMun = other.XMun;
            UF = other.UF;
            Nome = other.Nome;
            InscricaoEstadual = other.InscricaoEstadual;
            CPF = other.CPF;
            CNPJ = other.CNPJ;
        }

        public string CPF { get; set; }
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string Nome { get; set; }

        [XmlElement(ElementName = "IE")]
        public string InscricaoEstadual { get; set; }
        /// <summary>
        /// (Opcional)
        /// endereco Completo.
        /// </summary>
        [XmlElement("xEnder ")]
        public string XEnder { get; set; }

        /// <summary>
        /// (Opcional)
        /// Nome do município.
        /// </summary>
        [XmlElement("xMun")]
        public string XMun { get; set; }

        /// <summary>
        /// (Opcional)
        /// Sigla da UF.
        /// Informar "EX" para Exterior.
        /// </summary>
        public string UF { get; set; }

        [XmlIgnore]
        public string Documento => CNPJ ?? CPF;
        [XmlIgnore]
        public TiposDocumento TipoDocumento => !string.IsNullOrEmpty(CNPJ) ? TiposDocumento.CNPJ : TiposDocumento.CPF;
    }
}
