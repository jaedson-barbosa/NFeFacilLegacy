using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo de Partilha do ICMS entre a UF de origem e UF de destino ou a UF definida na legislação. 
    /// </summary>
    public class ICMSPart : ComumICMS, IRegimeNormal
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
        /// Percentual da BC operação própria.
        /// </summary>
        [XmlElement(Order = 13)]
        public string pBCOp { get; set; }

        /// <summary>
        /// UF para qual é devido o ICMS ST.
        /// </summary>
        [XmlElement(Order = 14)]
        public string UFST { get; set; }
    }
}
