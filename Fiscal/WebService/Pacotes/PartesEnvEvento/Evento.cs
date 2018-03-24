using NFeFacil.Certificacao;
using BaseGeral.ModeloXML.PartesAssinatura;
using System.Xml;
using System.Xml.Serialization;
using BaseGeral.Certificacao;

namespace NFeFacil.WebService.Pacotes.PartesEnvEvento
{
    [XmlRoot("evento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class Evento : ISignature
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("infEvento", Order = 0)]
        public InformacoesEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#", Order = 1)]
        public Assinatura Signature { get; set; }

        public Evento() { }
        public Evento(string versao, InformacoesEvento infEvento)
        {
            Versao = versao;
            InfEvento = infEvento;
        }
    }
}
