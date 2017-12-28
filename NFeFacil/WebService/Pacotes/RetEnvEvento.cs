using NFeFacil.WebService.Pacotes.PartesRetEnvEvento;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("retEnvEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct RetEnvEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public int IdLote { get; set; }

        [XmlElement("tpAmb")]
        public int TpAmb { get; set; }

        [XmlElement("verAplic")]
        public string VerAplic { get; set; }

        [XmlElement("cOrgao")]
        public int COrgao { get; set; }

        [XmlElement("cStat")]
        public int CStat { get; set; }

        [XmlElement("xMotivo")]
        public string XMotivo { get; set; }

        [XmlElement("retEvento")]
        public ResultadoEvento[] RetEvento { get; set; }
    }
}
