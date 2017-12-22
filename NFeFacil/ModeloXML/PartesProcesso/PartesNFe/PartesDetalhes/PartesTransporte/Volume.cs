using NFeFacil.AtributosVisualizacao;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class Volume
    {
        [XmlElement("qVol", Order = 0), DescricaoPropriedade("Quantidade")]
        public string QVol { get; set; }

        [XmlElement("esp", Order = 1), DescricaoPropriedade("Espécie")]
        public string Esp { get; set; }

        [XmlElement("marca", Order = 2), DescricaoPropriedade("Marca")]
        public string Marca { get; set; }

        [XmlElement("nVol", Order = 3), DescricaoPropriedade("Numeração")]
        public string NVol { get; set; }

        [XmlElement("pesoL", Order = 4), DescricaoPropriedade("Peso líquido")]
        public double PesoL { get; set; }

        [XmlElement("pesoB", Order = 5), DescricaoPropriedade("Peso bruto")]
        public double PesoB { get; set; }

        [XmlElement("lacres", Order = 6)]
        public List<Lacre> Lacres { get; set; } = new List<Lacre>();
    }

    public struct Lacre
    {
        [XmlElement("nLacre"), DescricaoPropriedade("Número do lacre")]
        public string NLacre { get; set; }
    }
}
