using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS90 : ComumICMS, IRegimeNormal
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
        /// Valor da BC do ICMS.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vBC { get; set; }

        /// <summary>
        /// Percentual da Redução de BC.
        /// </summary>
        [XmlElement(Order = 4)]
        public string pRedBC { get; set; }

        /// <summary>
        /// Alíquota do imposto.
        /// </summary>
        [XmlElement(Order = 5)]
        public string pICMS { get; set; }

        /// <summary>
        /// Valor do ICMS.
        /// </summary>
        [XmlElement(Order = 6)]
        public string vICMS { get; set; }

        /// <summary>
        /// Modalidade de determinação da BC do ICMS ST.
        /// </summary>
        [XmlElement(Order = 7)]
        public string modBCST { get; set; }

        /// <summary>
        /// Percentual da margem de valor Adicionado do ICMS ST.
        /// </summary>
        [XmlElement(Order = 8)]
        public string pMVAST { get; set; }

        /// <summary>
        /// Percentual da Redução de BC do ICMS ST.
        /// </summary>
        [XmlElement(Order = 9)]
        public string pRedBCST { get; set; }

        /// <summary>
        /// Valor da BC do ICMS ST.
        /// </summary>
        [XmlElement(Order = 10)]
        public string vBCST { get; set; }

        /// <summary>
        /// Alíquota do imposto do ICMS ST.
        /// </summary>
        [XmlElement(Order = 11)]
        public string pICMSST { get; set; }

        /// <summary>
        /// Valor do ICMS ST.
        /// </summary>
        [XmlElement(Order = 12)]
        public string vICMSST { get; set; }

        /// <summary>
        /// Valor do ICMS da desoneração.
        /// </summary>
        [XmlElement(Order = 13)]
        public string vICMSDeson { get; set; }

        /// <summary>
        /// Motivo da desoneração do ICMS.
        /// </summary>
        [XmlElement(Order = 14)]
        public string motDesICMS { get; set; }
    }
}
