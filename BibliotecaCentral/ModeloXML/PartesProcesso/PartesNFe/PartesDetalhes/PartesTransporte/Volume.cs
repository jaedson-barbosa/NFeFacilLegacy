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
        public string qVol { get; set; }

        /// <summary>
        /// (Opcional)
        /// Espécie dos volumes transportados.
        /// </summary>
        public string esp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Marca dos volumes transportados.
        /// </summary>
        public string marca { get; set; }

        /// <summary>
        /// (Opcional)
        /// Numeração dos volumes transportados.
        /// </summary>
        public string nVol { get; set; }

        /// <summary>
        /// (Opcional)
        /// Peso Líquido (em kg).
        /// </summary>
        public string pesoL { get; set; }

        /// <summary>
        /// (Opcional)
        /// Peso Bruto (em kg).
        /// </summary>
        public string pesoB { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo lacres.
        /// </summary>
        [XmlElement(nameof(lacres))]
        public List<Lacre> lacres = new List<Lacre>();
    }

    public struct Lacre
    {
        /// <summary>
        /// Número do lacre.
        /// </summary>
        public string nLacre { get; set; }
    }
}
