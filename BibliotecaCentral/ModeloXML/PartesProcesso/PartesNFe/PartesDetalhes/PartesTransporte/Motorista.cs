using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public sealed class Motorista
    {
        [XmlElement(Order = 0)]
        public string CPF { get; set; }

        [XmlElement(Order = 1)]
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "xNome", Order = 2)]
        public string Nome { get; set; }

        [XmlElement(ElementName = "IE", Order = 3)]
        public string InscricaoEstadual { get; set; }
        /// <summary>
        /// (Opcional)
        /// endereco Completo.
        /// </summary>
        [XmlElement("xEnder", Order = 4)]
        public string XEnder { get; set; }

        /// <summary>
        /// (Opcional)
        /// Nome do município.
        /// </summary>
        [XmlElement("xMun", Order = 5)]
        public string XMun { get; set; }

        /// <summary>
        /// (Opcional)
        /// Sigla da UF.
        /// Informar "EX" para Exterior.
        /// </summary>
        [XmlElement(Order = 6)]
        public string UF { get; set; }

        [XmlIgnore]
        public string Documento => CNPJ ?? CPF;
    }
}
