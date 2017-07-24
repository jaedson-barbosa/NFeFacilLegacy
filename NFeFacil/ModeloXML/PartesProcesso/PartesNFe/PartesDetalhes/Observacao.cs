using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Observacao
    {
        /// <summary>
        /// Identificação do campo.
        /// </summary>
        [XmlElement("xCampo", Order = 0)]
        public string XCampo { get; set; }

        /// <summary>
        /// Conteúdo do campo.
        /// </summary>
        [XmlElement("xTexto", Order = 1)]
        public string XTexto { get; set; }
    }
}
