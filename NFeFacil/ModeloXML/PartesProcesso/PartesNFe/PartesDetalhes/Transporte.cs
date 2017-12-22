using NFeFacil.AtributosVisualizacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Transporte
    {
        [XmlElement("modFrete", Order = 0), DescricaoPropriedade("Modalidade de frete")]
        public ushort ModFrete { get; set; }

        [XmlElement("transporta", Order = 1), DescricaoPropriedade("Motorista")]
        public Motorista Transporta { get; set; }

        [XmlElement("retTransp", Order = 2), DescricaoPropriedade("Retenção do transporte")]
        public ICMSTransporte RetTransp { get; set; }

        [XmlElement("veicTransp", Order = 3), DescricaoPropriedade("Veículo")]
        public Veiculo VeicTransp { get; set; }

        [XmlElement("reboque", Order = 4)]
        public List<Reboque> Reboque { get; set; } = new List<Reboque>();

        [XmlElement("vagao", Order = 5), DescricaoPropriedade("Vagão")]
        public string Vagao { get; set; }

        [XmlElement("balsa", Order = 6), DescricaoPropriedade("Balsa")]
        public string Balsa { get; set; }

        [XmlElement("vol", Order = 7)]
        public List<Volume> Vol { get; set; } = new List<Volume>();
    }
}
