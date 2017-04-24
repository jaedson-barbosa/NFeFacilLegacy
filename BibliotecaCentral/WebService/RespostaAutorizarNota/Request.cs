using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    [XmlRoot("nfeDadosMsg", Namespace = ConjuntoServicos.RespostaAutorizarServico)]
    public struct Request
    {
        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public CorpoRequest consReciNFe { get; set; }
    }
}
