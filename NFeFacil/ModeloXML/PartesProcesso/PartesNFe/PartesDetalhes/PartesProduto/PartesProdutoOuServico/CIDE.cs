using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class CIDE
    {
        /// <summary>
        /// Informar a BC da CIDE em quantidade.
        /// </summary>
        [XmlElement("qBCProd", Order = 0)]
        public double QBCProd { get; set; }

        /// <summary>
        /// Valor da alíquota da CIDE.
        /// </summary>
        [XmlElement("vAliqProd", Order = 1)]
        public double VAliqProd { get; set; }

        /// <summary>
        /// Valor da CIDE.
        /// </summary>
        [XmlElement("vCIDE", Order = 2)]
        public double VCIDE { get; set; }
    }
}