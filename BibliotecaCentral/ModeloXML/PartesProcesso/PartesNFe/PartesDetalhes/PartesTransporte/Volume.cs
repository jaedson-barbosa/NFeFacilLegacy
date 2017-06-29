using System.Collections.Generic;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class Volume
    {
        /// <summary>
        /// (Opcional)
        /// Quantidade de volumes transportados.
        /// </summary>
        [XmlElement("qVol", Order = 0)]
        public long QVol { get; set; }

        /// <summary>
        /// (Opcional)
        /// Espécie dos volumes transportados.
        /// </summary>
        [XmlElement("esp", Order = 1)]
        public string Esp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Marca dos volumes transportados.
        /// </summary>
        [XmlElement("marca", Order = 2)]
        public string Marca { get; set; }

        /// <summary>
        /// (Opcional)
        /// Numeração dos volumes transportados.
        /// </summary>
        [XmlElement("nVol", Order = 3)]
        public string NVol { get; set; }

        /// <summary>
        /// (Opcional)
        /// Peso Líquido (em kg).
        /// </summary>
        [XmlElement("pesoL", Order = 4)]
        public double PesoL { get; set; }

        /// <summary>
        /// (Opcional)
        /// Peso Bruto (em kg).
        /// </summary>
        [XmlElement("pesoB", Order = 5)]
        public double PesoB { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo lacres.
        /// </summary>
        [XmlElement("lacres", Order = 6]
        public List<Lacre> Lacres { get; set; } = new List<Lacre>();
    }

    public struct Lacre
    {
        /// <summary>
        /// Número do lacre.
        /// </summary>
        [XmlElement("nLacre")]
        public string NLacre { get; set; }
    }
}
