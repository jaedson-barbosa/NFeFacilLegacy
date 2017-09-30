using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS Substituição Tributária.
    /// </summary>
    public sealed class PISST : Imposto
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Valor da Base de Cálculo do PIS")]
        public string vBC { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Alíquota do PIS (em percentual)")]
        public string pPIS { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Quantidade Vendida")]
        public string qBCProd { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Alíquota do PIS (em reais)")]
        public string vAliqProd { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Valor do PIS")]
        public string vPIS { get; set; }

        public override bool IsValido => NaoNulos(vBC, pPIS, qBCProd, vAliqProd, vPIS);
    }
}
