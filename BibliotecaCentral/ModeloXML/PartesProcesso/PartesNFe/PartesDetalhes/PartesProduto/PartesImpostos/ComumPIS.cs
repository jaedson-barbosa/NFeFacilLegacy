using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
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
