namespace NFeFacil.DANFE.Pacotes
{
    public sealed class DadosNFe
    {
        public string NomeEmitente;
        public string TipoEmissao;
        public string NumeroNota;
        public string SerieNota;
        public string PaginaAtual;
        public string QuantPaginas;
        public string Chave;
        public string NumeroProtocolo;
        public string DataHoraRecibo;
        public string NatOp;
        public string IE;
        public string CNPJEmit;
        public Endereço Endereco;

        public void DefinirPagina(int totPagina, int paginaAtual)
        {
            QuantPaginas = totPagina.ToString();
            PaginaAtual = paginaAtual.ToString();
        }
    }
}
