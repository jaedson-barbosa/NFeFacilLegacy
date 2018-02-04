using NFeFacil.WebService.Pacotes.PartesRetEnviNFe;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("retEnviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class RetEnviNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public int TipoAmbiente { get; set; }

        [XmlElement("verAplic", Order = 1)]
        public string VersaoAplicativo { get; set; }

        [XmlElement("cStat", Order = 2)]
        public int StatusResposta { get; set; }

        [XmlElement("xMotivo", Order = 3)]
        public string DescricaoResposta { get; set; }

        [XmlElement("cUF", Order = 4)]
        public ushort Estado { get; set; }

        [XmlElement("dhRecbto", Order = 5)]
        public string DataHoraProcessamento { get; set; }

        [XmlElement("infRec", Order = 6)]
        public ReciboLote DadosRecibo { get; set; }
    }
}
