using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS Outras Operações.
    /// </summary>
    public sealed class COFINSOutr : ComumCOFINS
    {
        [DescricaoPropriedade("Valor da Base de Cálculo da COFINS")]
        [XmlElement(Order = 1)]
        public string vBC { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em percentual)")]
        [XmlElement(Order = 2)]
        public string pCOFINS { get; set; }

        [DescricaoPropriedade("Quantidade Vendida")]
        [XmlElement(Order = 3)]
        public string qBCProd { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em reais)")]
        [XmlElement(Order = 4)]
        public string vAliqProd { get; set; }

        [DescricaoPropriedade("Valor da COFINS")]
        [XmlElement(Order = 5)]
        public string vCOFINS { get; set; }
    }
}
