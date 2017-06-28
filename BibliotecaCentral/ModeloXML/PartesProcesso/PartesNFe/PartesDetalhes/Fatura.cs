using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Fatura
    {
        /// <summary>
        /// Número da Fatura.
        /// </summary>
        [XmlElement("nFat")]
        public string NFat { get; set; }

        /// <summary>
        /// Valor Original da Fatura.
        /// </summary>
        [XmlElement("vOrig")]
        public double VOrig { get; set; }

        /// <summary>
        /// Valor do desconto.
        /// </summary>
        [XmlElement("vDesc")]
        public double VDesc { get; set; }

        /// <summary>
        /// Valor Líquido da Fatura.
        /// </summary>
        [XmlElement("vLiq")]
        public double VLiq { get; set; }
    }
}
