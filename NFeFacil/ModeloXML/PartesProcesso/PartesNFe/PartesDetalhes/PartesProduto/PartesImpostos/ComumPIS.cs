using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS.
    /// </summary>
    public class ComumPIS
    {
        /// <summary>
        /// Código de Situação Tributária do PIS.
        /// </summary>
        [XmlElement(Order = 0)]
        public string CST;
    }
}
