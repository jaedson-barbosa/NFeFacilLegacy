using NFeFacil.Certificacao;
using NFeFacil.ModeloXML.PartesAssinatura;
using NFeFacil.WebService.Pacotes.PartesInutNFe;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("inutNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class InutNFe : ISignature
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("infInut", Order = 0)]
        public InfInut Info { get; set; }

        [XmlElement("Signature", Order = 1, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }

        public InutNFe() { }
        public InutNFe(InfInut info)
        {
            Versao = "3.10";
            Info = info;
            Signature = null;
        }

        public async Task<(bool, string)> PrepararEvento(AssinaFacil assinador, object cert)
        {
            assinador.Nota = this;
            return await assinador.Assinar(cert, Info.Id, "infInut");
        }
    }
}
