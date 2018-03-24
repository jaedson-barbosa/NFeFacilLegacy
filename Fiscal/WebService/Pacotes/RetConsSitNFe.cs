using BaseGeral.ModeloXML;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("retConsSitNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct RetConsSitNFe
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
        public int Estado { get; set; }

        [XmlElement("dhRecbto", Order = 5)]
        public string DataHoraProcessamento { get; set; }

        [XmlElement("chNFe", Order = 6)]
        public string ChaveNFe { get; set; }

        [XmlElement("protNFe", Order = 7)]
        public ProtocoloNFe Protocolo { get; set; }
    }
}
