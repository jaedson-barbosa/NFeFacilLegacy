using BaseGeral;
using System.Xml;
using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes.PartesEnvEvento
{
    [XmlRoot("infEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct InformacoesEvento
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("cOrgao", Order = 0)]
        public int COrgao { get; set; }

        [XmlElement("tpAmb", Order = 1)]
        public int TpAmb { get; set; }

        [XmlElement(Order = 2)]
        public string CNPJ { get; set; }

        [XmlElement("chNFe", Order = 3)]
        public string ChNFe { get; set; }

        [XmlElement("dhEvento", Order = 4)]
        public string DhEvento { get; set; }

        [XmlElement("tpEvento", Order = 5)]
        public int TpEvento { get; set; }

        [XmlElement("nSeqEvento", Order = 6)]
        public int NSeqEvento { get; set; }

        [XmlElement("verEvento", Order = 7)]
        public string VerEvento { get; set; }

        [XmlElement("detEvento", Order = 8)]
        public DetalhamentoEvento DetEvento { get; set; }

        public InformacoesEvento(int cOrgao, string CNPJ, string chaveNFe, ulong numeroProtocolo, string justificativa, int tipoAmbiente)
        {
            COrgao = cOrgao;
            TpAmb = tipoAmbiente;
            this.CNPJ = CNPJ;
            ChNFe = chaveNFe;
            DhEvento = DefinicoesTemporarias.DateTimeNow.ToStringPersonalizado();
            TpEvento = 110111;
            NSeqEvento = 1;
            VerEvento = "1.00";
            DetEvento = new DetalhamentoEvento("1.00", numeroProtocolo, justificativa);
            Id = "ID" + TpEvento + chaveNFe + NSeqEvento.ToString("00");
        }
    }
}
