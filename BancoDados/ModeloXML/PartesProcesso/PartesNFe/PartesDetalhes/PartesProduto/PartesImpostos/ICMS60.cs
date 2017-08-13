using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS60 : ComumICMS, IRegimeNormal
    {
        /// <summary>
        /// Tributação do ICMS.
        /// </summary>
        [XmlElement(Order = 1)]
        public string CST { get; set; }

        /// <summary>
        /// Valor da BC do ICMS Retido Anteriormente.
        /// </summary>
        [XmlElement(Order = 2)]
        public string vBCSTRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido Anteriormente.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vICMSSTRet { get; set; }
    }
}
