using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes.PartesRetEnviNFe
{
    public struct ReciboLote
    {
        [XmlElement("nRec", Order = 0)]
        public string NumeroRecibo { get; set; }

        [XmlElement("tMed", Order = 1)]
        public int TempoMedioResposta { get; set; }
    }
}
