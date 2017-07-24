using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class ProcessoReferenciado
    {
        /// <summary>
        /// Identificador do processo ou ato concessório.
        /// </summary>
        [XmlElement("nProc", Order = 0)]
        public string NProc { get; set; }

        /// <summary>
        /// Indicador da origem do processo.
        /// </summary>
        [XmlElement("indProc", Order = 1)]
        public int IndProc { get; set; }
    }
}
