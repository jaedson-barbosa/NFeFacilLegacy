using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    [XmlRoot("nfeDadosMsg", Namespace = EnderecosConexao.ConsultarServico)]
    public struct Request
    {
        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public CorpoRequest consSitNFe { get; set; }
    }
}
