using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS Outras Operações;
    /// </summary>
    public sealed class PISOutr : ComumPIS
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Valor da Base de Cálculo do PIS")]
        public string vBC { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Alíquota do PIS (em percentual)")]
        public string pPIS { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Quantidade Vendida")]
        public string qBCProd { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Alíquota do PIS (em reais)")]
        public string vAliqProd { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Valor do PIS")]
        public string vPIS { get; set; }
    }
}
