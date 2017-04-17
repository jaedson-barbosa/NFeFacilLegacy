namespace BibliotecaCentral.Validacao
{
    public struct ConjuntoAnalise
    {
        public bool EstáErrado;
        public string Mensagem;

        public ConjuntoAnalise(bool estaErrado, string mensagem)
        {
            EstáErrado = estaErrado;
            Mensagem = mensagem;
        }
    }
}
