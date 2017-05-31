namespace NFeFacil.DANFE.Pacotes
{
    public sealed class DadosNFe
    {
        public string NomeEmitente { get; set; }
        public string TipoEmissao { get; set; }
        public string NumeroNota { get; set; }
        public string SerieNota { get; set; }
        public string PaginaAtual { get; set; }
        public string QuantPaginas { get; set; }
        public string Chave { get; set; }
        public string ChaveComMascara { get; set; }
        public string NumeroProtocolo { get; set; }
        public string DataHoraRecibo { get; set; }
        public string NatOp { get; set; }
        public string IE { get; set; }
        public string IEST { get; set; }
        public string CNPJEmit { get; set; }
        public Endereço Endereco { get; set; }

        public void DefinirPagina(int totPagina, int paginaAtual)
        {
            QuantPaginas = totPagina.ToString();
            PaginaAtual = paginaAtual.ToString();
        }
    }
}
