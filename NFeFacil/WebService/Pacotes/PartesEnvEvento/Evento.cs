using NFeFacil.Certificacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes.PartesEnvEvento
{
    [XmlRoot("evento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class Evento : ISignature
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("infEvento")]
        public InformacoesEvento InfEvento { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }

        public Evento() { }
        public Evento(string versao, InformacoesEvento infEvento)
        {
            Versao = versao;
            InfEvento = infEvento;
        }

        public async Task Preparar()
        {
            await new AssinaFacil(this).Assinar<Evento>(InfEvento.Id, "infEvento");
        }
    }
}
