using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Transporte
    {
        public int modFrete { get; set; }
        public string vagao { get; set; }
        public string balsa { get; set; }

        public Motorista transporta;

        public Veiculo veicTransp;

        public ICMSTransporte retTransp;

        [XmlElement(nameof(reboque))]
        public List<Reboque> reboque = new List<Reboque>();

        [XmlElement(nameof(vol))]
        public List<Volume> vol = new List<Volume>();
    }
}
