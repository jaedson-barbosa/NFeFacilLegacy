using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS Substituição Tributária.
    /// </summary>
    public sealed class PISST : Imposto
    {
        /// <summary>
        /// Valor da Base de Cálculo do PIS.
        /// </summary>
        [XmlElement(Order = 0)]
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do PIS (em percentual).
        /// </summary>
        [XmlElement(Order = 1)]
        public string pPIS { get; set; }

        /// <summary>
        /// Quantidade Vendida.
        /// </summary>
        [XmlElement(Order = 2)]
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota do PIS (em reais).
        /// </summary>
        [XmlElement(Order = 3)]
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor do PIS.
        /// </summary>
        [XmlElement(Order = 4)]
        public string vPIS { get; set; }

        public override bool IsValido => NaoNulos(vBC, pPIS, qBCProd, vAliqProd, vPIS);
    }
}
