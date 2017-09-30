using NFeFacil.ModeloXML.PartesProcesso;
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

    public struct ResultadoEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("infEvento")]
        public InformacoesRegistroEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }
    }

    public struct InformacoesRegistroEvento
    {
        [XmlAttribute]
        public string Id { get; set; }

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

        [XmlElement("chNFe")]
        public string ChNFe { get; set; }

        [XmlElement("tpEvento")]
        public int TpEvento { get; set; }

        [XmlElement("xEvento")]
        public string XEvento { get; set; }

        [XmlElement("nSeqEvento")]
        public int NSeqEvento { get; set; }

        public string CNPJDest { get; set; }
        public string CPFDest { get; set; }

        [XmlElement("emailDest")]
        public string EmailDest { get; set; }

        [XmlElement("dhRegEvento")]
        public string DhRegEvento { get; set; }

        [XmlElement("nProt")]
        public ulong NProt { get; set; }
    }
}
