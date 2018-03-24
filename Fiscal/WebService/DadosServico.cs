namespace NFeFacil.WebService
{
    public struct DadosServico
    {
        public string Endereco { get; }
        public string Servico { get; }
        public string Metodo { get; }

        internal DadosServico(string endereco, string servico, string metodo)
        {
            Endereco = endereco;
            Servico = servico;
            Metodo = metodo;
        }
    }
}
