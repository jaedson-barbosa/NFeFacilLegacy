using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Fatura
    {
        /// <summary>
        /// Número da Fatura.
        /// </summary>
        [XmlElement("nFat", Order = 0)]
        public string NFat { get; set; }

        /// <summary>
        /// Valor Original da Fatura.
        /// </summary>
        [XmlElement("vOrig", Order = 1)]
        public double VOrig { get; set; }

        /// <summary>
        /// Valor do desconto.
        /// </summary>
        [XmlElement("vDesc", Order = 2)]
        public double VDesc { get; set; }

        /// <summary>
        /// Valor Líquido da Fatura.
        /// </summary>
        [XmlElement("vLiq", Order = 3)]
        public double VLiq { get; set; }
    }
}
