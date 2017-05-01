using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    [XmlRoot("nfeConsultaNF2Result", Namespace = EnderecosConexao.ConsultarServico)]
    public struct Response
    {
        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public CorpoResponse retConsSitNFe { get; set; }
    }
}
