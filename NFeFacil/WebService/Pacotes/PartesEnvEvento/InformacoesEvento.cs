using System.Xml;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes.PartesEnvEvento
{
    [XmlRoot("infEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct InformacoesEvento
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("cOrgao")]
        public int COrgao { get; set; }

        [XmlElement("tpAmb")]
        public int TpAmb { get; set; }

        [XmlElement]
        public string CNPJ { get; set; }

        [XmlElement("chNFe")]
        public string ChNFe { get; set; }

        [XmlElement("dhEvento")]
        public string DhEvento { get; set; }

        [XmlElement("tpEvento")]
        public int TpEvento { get; set; }

        [XmlElement("nSeqEvento")]
        public int NSeqEvento { get; set; }

        [XmlElement("verEvento")]
        public string VerEvento { get; set; }

        [XmlElement("detEvento")]
        public DetalhamentoEvento DetEvento { get; set; }

        public InformacoesEvento(int cOrgao, string CNPJ, string chaveNFe, string versaoEvento, ulong numeroProtocolo, string justificativa, int tipoAmbiente)
        {
            COrgao = cOrgao;
            TpAmb = tipoAmbiente;
            this.CNPJ = CNPJ;
            ChNFe = chaveNFe;
            DhEvento = DefinicoesTemporarias.DateTimeNow.ToStringPersonalizado();
            TpEvento = 110111;
            NSeqEvento = 1;
            VerEvento = versaoEvento;
            DetEvento = new DetalhamentoEvento(versaoEvento, numeroProtocolo, justificativa);
            Id = "ID" + TpEvento + chaveNFe + NSeqEvento.ToString("00");
        }
    }
}
