using System.Xml.Linq;

namespace NFeFacil.Certificacao.LAN.Pacotes
{
    public struct RequisicaoEnvioDTO
    {
        public XElement Conteudo { get; set; }
        public CabecalhoRequisicao Cabecalho { get; set; }
        public string Uri { get; set; }
    }

    public struct CabecalhoRequisicao
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}
