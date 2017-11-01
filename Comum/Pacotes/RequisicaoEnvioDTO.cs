namespace Comum.Pacotes
{
    public struct RequisicaoEnvioDTO
    {
        public string Conteudo { get; set; }
        public CabecalhoRequisicao Cabecalho { get; set; }
        public string Uri { get; set; }
    }

    public struct CabecalhoRequisicao
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}
