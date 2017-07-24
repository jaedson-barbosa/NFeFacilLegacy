using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS tributado por quantidade.
    /// </summary>
    public sealed class PISQtde : ComumPIS
    {
        /// <summary>
        /// Quantidade Vendida.
        /// </summary>
        [XmlElement(Order = 1)]
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota do PIS (em reais).
        /// </summary>
        [XmlElement(Order = 2)]
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor do PIS.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vPIS { get; set; }
    }
}
