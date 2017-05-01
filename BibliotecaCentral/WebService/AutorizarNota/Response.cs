using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    [XmlRoot("nfeAutorizacaoLoteResult", Namespace = EnderecosConexao.AutorizarServico)]
    public struct Response
    {
        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public CorpoResponse retEnviNFe { get; set; }
    }
}
