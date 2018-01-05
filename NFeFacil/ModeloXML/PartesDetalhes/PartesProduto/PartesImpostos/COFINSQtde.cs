using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo de COFINS tributado por quantidade
    /// </summary>
    public sealed class COFINSQtde : ComumCOFINS
    {
        [DescricaoPropriedade("Quantidade Vendida")]
        [XmlElement(Order = 1)]
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota da COFINS (em reais).
        /// </summary>
        [XmlElement(Order = 2)]
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor da COFINS.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vCOFINS { get; set; }
    }
}
