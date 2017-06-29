using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Deducoes
    {
        /// <summary>
        /// Descrição da Dedução.
        /// </summary>
        [XmlElement("xDed", Order = 0)]
        public string XDed { get; set; }

        /// <summary>
        /// Valor da Dedução.
        /// </summary>
        [XmlElement("vDed", Order = 1)]
        public double VDed { get; set; }
    }
}
