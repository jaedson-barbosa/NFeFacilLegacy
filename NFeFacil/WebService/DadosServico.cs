namespace NFeFacil.WebService
{
    public struct DadosServico
    {
        public string Endereco { get; }
        public string Servico { get; }
        public string Metodo { get; }

        public string VersaoRecepcaoEvento { get; }

        internal DadosServico(string endereco, string servico, string metodo, string versaoEvento)
        {
            Endereco = endereco;
            Servico = servico;
            Metodo = metodo;
            VersaoRecepcaoEvento = versaoEvento;
        }
    }
}
