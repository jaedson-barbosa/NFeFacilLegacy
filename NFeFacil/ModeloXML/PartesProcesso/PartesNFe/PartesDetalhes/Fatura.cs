using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Fatura
    {
        /// <summary>
        /// (Opcional)
        /// Número da Fatura.
        /// </summary>
        [XmlElement("nFat")]
        public string NFat { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Original da Fatura.
        /// </summary>
        [XmlElement("vOrig")]
        public string VOrig { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor do desconto.
        /// </summary>
        [XmlElement("vDesc")]
        public string VDesc { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Líquido da Fatura.
        /// </summary>
        [XmlElement("vLiq")]
        public string VLiq { get; set; }
    }
}
