using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS51 : ComumICMS, IRegimeNormal
    {
        /// <summary>
        /// Tributação do ICMS.
        /// </summary>
        [XmlElement(Order = 1)]
        public string CST { get; set; }

        /// <summary>
        /// Modalidade de determinação da BC do ICMS.
        /// </summary>
        [XmlElement(Order = 2)]
        public string modBC { get; set; }

        /// <summary>
        /// Percentual da Redução de BC.
        /// </summary>
        [XmlElement(Order = 3)]
        public string pRedBC { get; set; }

        /// <summary>
        /// Valor da BC do ICMS.
        /// </summary>
        [XmlElement(Order = 4)]
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do imposto.
        /// </summary>
        [XmlElement(Order = 5)]
        public string pICMS { get; set; }

        /// <summary>
        /// Valor do ICMS da Operação.
        /// </summary>
        [XmlElement(Order = 6)]
        public string vICMSOp { get; set; }

        /// <summary>
        /// Percentual do diferimento.
        /// </summary>
        [XmlElement(Order = 7)]
        public string pDif { get; set; }

        /// <summary>
        /// Valor do ICMS Diferido.
        /// </summary>
        [XmlElement(Order = 8)]
        public string vICMSDif { get; set; }

        /// <summary>
        /// Valor do ICMS.
        /// </summary>
        [XmlElement(Order = 9)]
        public string vICMS { get; set; }
    }
}
