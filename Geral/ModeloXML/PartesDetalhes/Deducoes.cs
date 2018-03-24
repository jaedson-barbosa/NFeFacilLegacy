using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes
{
    public sealed class Deducoes
    {
        /// <summary>
        /// Descrição da Dedução.
        /// </summary>
        [XmlElement("xDed", Order = 0), DescricaoPropriedade("Descrição")]
        public string XDed { get; set; }

        /// <summary>
        /// Valor da Dedução.
        /// </summary>
        [XmlElement("vDed", Order = 1), DescricaoPropriedade("Valor")]
        public double VDed { get; set; }
    }
}
