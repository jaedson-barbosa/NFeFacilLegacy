using System.Xml.Serialization;

namespace NFeFacil.WebService.ConsultarNota
{
    [XmlRoot("nfeConsultaNF2Result", Namespace = ConjuntoServicos.ConsultarServico)]
    public struct Response
    {
        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public CorpoResponse retConsSitNFe { get; set; }
    }
}
