using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Transporte
    {
        public int modFrete { get; set; }
        public string vagao { get; set; }
        public string balsa { get; set; }

        public Motorista transporta { get; set; } = new Motorista();
        public Veiculo veicTransp { get; set; } = new Veiculo();
        public ICMSTransporte retTransp { get; set; } = new ICMSTransporte();

        [XmlElement(nameof(reboque))]
        public List<Reboque> reboque { get; set; } = new List<Reboque>();

        [XmlElement(nameof(vol))]
        public List<Volume> vol { get; set; } = new List<Volume>();
    }
}
