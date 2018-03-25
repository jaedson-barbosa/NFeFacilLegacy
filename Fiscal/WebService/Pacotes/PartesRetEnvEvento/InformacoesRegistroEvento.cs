using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes.PartesRetEnvEvento
{
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
