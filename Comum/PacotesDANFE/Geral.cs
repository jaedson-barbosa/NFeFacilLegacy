namespace Comum.PacotesDANFE
{
    public struct Geral
    {
        public DadosAdicionais _DadosAdicionais { get; set; }
        public DadosCabecalho _DadosCabecalho { get; set; }
        public DadosCliente _DadosCliente { get; set; }
        public DadosImposto _DadosImposto { get; set; }
        public DadosMotorista _DadosMotorista { get; set; }
        public DadosNFe _DadosNFe { get; set; }
        public DadosProduto[] _DadosProdutos { get; set; }
        public DadosDuplicata[] _Duplicatas { get; set; }
        public DadosISSQN _DadosISSQN { get; set; }
        public string Fatura { get; set; }
    }
}
