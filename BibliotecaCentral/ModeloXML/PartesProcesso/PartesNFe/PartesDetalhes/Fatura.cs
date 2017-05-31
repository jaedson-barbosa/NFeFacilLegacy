using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Fatura
    {
        /// <summary>
        /// (Opcional)
        /// Número da Fatura.
        /// </summary>
        [XmlElement("nFat")]
        public int NFat { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Original da Fatura.
        /// </summary>
        [XmlElement("vOrig")]
        public double VOrig { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor do desconto.
        /// </summary>
        [XmlElement("vDesc")]
        public double VDesc { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Líquido da Fatura.
        /// </summary>
        [XmlElement("vLiq")]
        public double VLiq { get; set; }
    }
}
