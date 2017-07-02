using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS tributado pela alíquota.
    /// </summary>
    public sealed class COFINSAliq : ComumCOFINS
    {
        /// <summary>
        /// Valor da Base de Cálculo da COFINS.
        /// </summary>
        [XmlElement(Order = 1)]
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota da COFINS (em percentual).
        /// </summary>
        [XmlElement(Order = 2)]
        public string pCOFINS { get; set; }

        /// <summary>
        /// Valor da COFINS.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vCOFINS { get; set; }
    }
}
