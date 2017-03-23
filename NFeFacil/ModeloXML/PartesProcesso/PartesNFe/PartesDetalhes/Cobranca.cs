using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Cobranca
    {
        [XmlElement("fat")]
        public Fatura Fat { get; set; }

        [XmlElement("dup")]
        public List<Duplicata> Dup { get; set; } = new List<Duplicata>();
    }

    public class Fatura
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

    public class Duplicata
    {
        /// <summary>
        /// (Opcional)
        /// Número da Duplicata.
        /// </summary>
        [XmlElement("nDup")]
        public string NDup { get; set; }

        /// <summary>
        /// (Opcional)
        /// Data de vencimento.
        /// Formato: “AAAA-MM-DD” 
        /// </summary>
        [XmlElement("dVenc")]
        public string DVenc { get; set; }

        /// <summary>
        /// Valor da duplicata.
        /// </summary>
        [XmlElement("dDup")]
        public double DDup { get; set; }
    }
}
