using System.Xml.Serialization;

namespace NFeFacil.WebService.ConsultarNota
{
    [XmlRoot("nfeDadosMsg", Namespace = ConjuntoServicos.ConsultarServico)]
    public struct Request
    {
        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public CorpoRequest consSitNFe { get; set; }
    }
}
