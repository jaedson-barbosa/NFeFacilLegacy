using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Transporte
    {
        [XmlElement("modFrete", Order = 0)]
        public byte ModFrete { get; set; }

        [XmlElement("transporta", Order = 1)]
        public Motorista Transporta { get; set; }

        [XmlElement("retTransp", Order = 2)]
        public ICMSTransporte RetTransp { get; set; }

        [XmlElement("veicTransp", Order = 3)]
        public Veiculo VeicTransp { get; set; }

        [XmlElement("reboque", Order = 4)]
        public List<Reboque> Reboque { get; set; } = new List<Reboque>();

        [XmlElement("vagao", Order = 5)]
        public string Vagao { get; set; }

        [XmlElement("balsa", Order = 6)]
        public string Balsa { get; set; }

        [XmlElement("vol", Order = 7)]
        public List<Volume> Vol { get; set; } = new List<Volume>();
    }
}
