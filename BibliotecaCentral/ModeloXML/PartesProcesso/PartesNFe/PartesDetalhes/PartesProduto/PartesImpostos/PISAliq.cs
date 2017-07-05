using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS tributado pela alíquota.
    /// </summary>
    public sealed class PISAliq : ComumPIS
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
        /// Valor do PIS.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vPIS { get; set; }
    }
}
