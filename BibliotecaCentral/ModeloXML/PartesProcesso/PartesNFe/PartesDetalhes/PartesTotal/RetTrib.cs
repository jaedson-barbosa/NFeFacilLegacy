using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
{
    public sealed class RetTrib
    {
        /// <summary>
        /// Informar o valor do PIS retido.
        /// </summary>
        [XmlElement("vRetPIS", Order = 0)]
        public double VRetPIS { get; set; }

        /// <summary>
        /// Informar o valor COFINS do retido.
        /// </summary>
        [XmlElement("vRetCOFINS", Order = 1)]
        public double VRetCOFINS { get; set; }

        /// <summary>
        /// Informar o valor do CSLL retido.
        /// </summary>
        [XmlElement("vRetCSLL", Order = 2)]
        public double VRetCSLL { get; set; }

        /// <summary>
        /// Informar o valor do BC IRRF retido.
        /// </summary>
        [XmlElement("vBCIRRF", Order = 3)]
        public double VBCIRRF { get; set; }

        /// <summary>
        /// Informar o valor do IRRF retido.
        /// </summary>
        [XmlElement("vIRRF", Order = 4)]
        public double VIRRF { get; set; }

        /// <summary>
        /// Informar o valor da BC da retenção da Previdência retido.
        /// </summary>
        [XmlElement("vBCRetPrev", Order = 5)]
        public double VBCRetPrev { get; set; }

        /// <summary>
        /// Informar o valor da retenção da Previdência retido.
        /// </summary>
        [XmlElement("vRetPrev", Order = 6)]
        public double VRetPrev { get; set; }
    }
}
