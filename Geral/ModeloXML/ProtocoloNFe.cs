using BaseGeral.Certificacao;
using BaseGeral.ModeloXML.PartesAssinatura;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML
{
    public sealed class ProtocoloNFe : ISignature
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; } = "3.10";

        [XmlElement("infProt", Order = 0)]
        public InfoProtocolo InfProt { get; set; }

        [XmlElement("Signature", Order = 1, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }

        internal async Task Assinar(object cert)
        {
            await new AssinaFacil()
            {
                Nota = this
            }.Assinar<ProtocoloNFe>(cert, InfProt.Id, "infProt");
        }
    }
}
