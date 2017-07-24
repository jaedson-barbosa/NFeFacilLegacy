using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public abstract class ComumIPI
    {
        /// <summary>
        /// Código da situação tributária do IPI.
        /// </summary>
        [XmlElement(Order = 0)]
        public string CST { get; set; }
    }
}
