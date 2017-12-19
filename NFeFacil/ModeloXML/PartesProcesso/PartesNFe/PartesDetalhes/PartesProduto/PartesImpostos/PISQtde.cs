using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS tributado por quantidade.
    /// </summary>
    public sealed class PISQtde : ComumPIS
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Quantidade Vendida")]
        public string qBCProd { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Alíquota do PIS (em reais)")]
        public string vAliqProd { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor do PIS")]
        public string vPIS { get; set; }
    }
}
