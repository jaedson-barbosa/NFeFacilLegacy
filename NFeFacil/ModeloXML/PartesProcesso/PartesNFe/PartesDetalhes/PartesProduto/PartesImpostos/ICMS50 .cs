using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo Tributação ICMS = 40, 41, 50.
    /// </summary>
    public class ICMS50 : ComumICMS, IRegimeNormal
    {
        /// <summary>
        /// Tributação do ICMS.
        /// </summary>
        [XmlElement(Order = 1)]
        public string CST { get; set; }

        /// <summary>
        /// Valor do ICMS da desoneração.
        /// </summary>
        [XmlElement(Order = 2)]
        public string vICMSDeson { get; set; }

        /// <summary>
        /// Motivo da desoneração do ICMS.
        /// </summary>
        [XmlElement(Order = 3)]
        public string motDesICMS { get; set; }
    }
}
