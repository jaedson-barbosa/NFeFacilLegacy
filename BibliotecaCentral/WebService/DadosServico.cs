namespace BibliotecaCentral.WebService
{
    internal struct DadosServico
    {
        internal string Endereco { get; }
        internal string Servico { get; }
        internal string Metodo { get; }

        internal DadosServico(string endereco, string servico, string metodo)
        {
            Endereco = endereco;
            Servico = servico;
            Metodo = metodo;
        }
    }
}
