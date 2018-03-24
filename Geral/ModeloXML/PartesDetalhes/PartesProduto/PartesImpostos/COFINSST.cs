using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS Substituição Tributária.
    /// Só deve ser informado se o Produto for sujeito a COFINS por ST (CST = 05).
    /// </summary>
    public class COFINSST : ImpostoBase
    {
        [DescricaoPropriedade("Valor da Base de Cálculo da COFINS")]
        [XmlElement(Order = 0)]
        public string vBC { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em percentual)")]
        [XmlElement(Order = 1)]
        public string pCOFINS { get; set; }

        [DescricaoPropriedade("Quantidade Vendida")]
        [XmlElement(Order = 2)]
        public string qBCProd { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em reais)")]
        [XmlElement(Order = 3)]
        public string vAliqProd { get; set; }

        [DescricaoPropriedade("Valor da COFINS")]
        [XmlElement(Order = 4)]
        public string vCOFINS { get; set; }
    }
}
