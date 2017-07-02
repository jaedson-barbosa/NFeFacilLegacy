using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS Outras Operações;
    /// </summary>
    public sealed class PISOutr : ComumPIS
    {
        /// <summary>
        /// Valor da Base de Cálculo do PIS.
        /// </summary>
        [XmlElement(Order = 1)]
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do PIS (em percentual).
        /// </summary>
        [XmlElement(Order = 2)]
        public string pPIS { get; set; }

        /// <summary>
        /// Quantidade Vendida.
        /// </summary>
        [XmlElement(Order = 3)]
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota do PIS (em reais).
        /// </summary>
        [XmlElement(Order = 4)]
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor do PIS.
        /// </summary>
        [XmlElement(Order = 5)]
        public string vPIS { get; set; }
    }
}
