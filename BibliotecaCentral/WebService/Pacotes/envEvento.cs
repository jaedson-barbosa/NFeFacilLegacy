using BibliotecaCentral.Certificacao;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.Pacotes
{
    [XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct EnvEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public int IdLote { get; set; }

        [XmlElement("evento")]
        public Evento[] Eventos { get; set; }

        public EnvEvento(string versao, params InformacoesEvento[] eventos)
        {
            Versao = versao;
            IdLote = 0;
            Eventos = new Evento[eventos.Length];
            for (int i = 0; i < eventos.Length; i++)
            {
                Eventos[i] = new Evento(versao, eventos[i]);
            }
        }
    }

    public sealed class Evento : ISignature
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("infEvento")]
        public InformacoesEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }

        public Evento(string versao, InformacoesEvento infEvento)
        {
            Versao = versao;
            InfEvento = infEvento;
            Task.Run(() => new AssinaFacil(this).Assinar(infEvento.Id)).Wait();
        }
    }

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
            this.COrgao = cOrgao;
            TpAmb = tipoAmbiente;
            this.CNPJ = CNPJ;
            ChNFe = chaveNFe;
            DhEvento = DateTime.Now.ToStringPersonalizado();
            TpEvento = 110111;
            NSeqEvento = 1;
            VerEvento = versaoEvento;
            DetEvento = new DetalhamentoEvento(versaoEvento, numeroProtocolo, justificativa);
            Id = "ID" + TpEvento + chaveNFe + NSeqEvento.ToString("00");
        }
    }

    public struct DetalhamentoEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("descEvento")]
        public string DescEvento { get; set; }

        [XmlElement("nProt")]
        public ulong NProt { get; set; }

        [XmlElement("xJust")]
        public string XJust { get; set; }

        public DetalhamentoEvento(string versao, ulong numeroProtocolo, string justificativa)
        {
            Versao = versao;
            DescEvento = "Cancelamento";
            NProt = numeroProtocolo;
            XJust = justificativa;
        }
    }
}
