using BaseGeral.ModeloXML;
using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes
{
    [XmlRoot("retConsReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct RetConsReciNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public int TipoAmbiente { get; set; }

        [XmlElement("verAplic", Order = 1)]
        public string VersaoAplicativo { get; set; }

        [XmlElement("nRec", Order = 2)]
        public string NumeroRecibo { get; set; }

        [XmlElement("cStat", Order = 3)]
        public int StatusResposta { get; set; }

        [XmlElement("xMotivo", Order = 4)]
        public string DescricaoResposta { get; set; }

        [XmlElement("cUF", Order = 5)]
        public int Estado { get; set; }

        [XmlElement("dhRecbto", Order = 6)]
        public string DataHoraProcessamento { get; set; }

        [XmlElement("protNFe", Order = 7)]
        public ProtocoloNFe Protocolo { get; set; }
    }
}
