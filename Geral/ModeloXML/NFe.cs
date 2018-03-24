using NFeFacil.Certificacao;
using NFeFacil.ModeloXML.PartesAssinatura;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML
{
    [XmlRoot(nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class NFe : ISignature
    {
        [XmlElement("infNFe")]
        public InformacoesNFe Informacoes { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Assinatura Signature { get; set; }

        [XmlIgnore]
        public bool AmbienteTestes => Informacoes.identificacao.TipoAmbiente == 2;
    }
}
