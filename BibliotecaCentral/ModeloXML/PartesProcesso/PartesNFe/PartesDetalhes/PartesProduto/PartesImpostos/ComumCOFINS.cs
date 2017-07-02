using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public abstract class ComumCOFINS
    {
        /// <summary>
        /// Código de Situação Tributária da COFINS.
        /// </summary>
        [XmlElement(Order = 0)]
        public string CST { get; set; }
    }
}
