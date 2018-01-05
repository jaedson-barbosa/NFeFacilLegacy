using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Fatura
    {
        /// <summary>
        /// Número da Fatura.
        /// </summary>
        [XmlElement("nFat", Order = 0), DescricaoPropriedade("Número")]
        public string NFat { get; set; }

        /// <summary>
        /// Valor Original da Fatura.
        /// </summary>
        [XmlElement("vOrig", Order = 1), DescricaoPropriedade("Valor original")]
        public double VOrig { get; set; }

        /// <summary>
        /// Valor do desconto.
        /// </summary>
        [XmlElement("vDesc", Order = 2), DescricaoPropriedade("Valor do desconto")]
        public double VDesc { get; set; }

        /// <summary>
        /// Valor Líquido da Fatura.
        /// </summary>
        [XmlElement("vLiq", Order = 3), DescricaoPropriedade("Valor líquido")]
        public double VLiq { get; set; }
    }
}
