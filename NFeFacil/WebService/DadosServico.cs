namespace NFeFacil.WebService
{
    internal struct DadosServico
    {
        internal string endereco { get; }
        internal string Servico { get; }
        internal string Metodo { get; }

        internal DadosServico(string endereco, string servico, string metodo)
        {
            this.endereco = endereco;
            Servico = servico;
            Metodo = metodo;
        }
    }
}
