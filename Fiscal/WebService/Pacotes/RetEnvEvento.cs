using Fiscal.WebService.Pacotes.PartesRetEnvEvento;
using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes
{
    [XmlRoot("retEnvEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct RetEnvEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote", Order = 0)]
        public int IdLote { get; set; }

        [XmlElement("tpAmb", Order = 1)]
        public int TipoAmbiente { get; set; }

        [XmlElement("verAplic", Order = 2)]
        public string VersaoAplicativo { get; set; }

        [XmlElement("cOrgao", Order = 3)]
        public int CodigoOrgao { get; set; }

        [XmlElement("cStat", Order = 4)]
        public int StatusResposta { get; set; }

        [XmlElement("xMotivo", Order = 5)]
        public string DescricaoResposta { get; set; }

        [XmlElement("retEvento", Order = 6)]
        public ResultadoEvento[] ResultadorEventos { get; set; }
    }
}
