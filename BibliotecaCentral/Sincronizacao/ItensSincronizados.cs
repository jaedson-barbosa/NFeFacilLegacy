namespace BibliotecaCentral.Sincronizacao
{
    internal struct ItensSincronizados
    {
        public int Enviados { get; }
        public int Recebidos { get; }

        public ItensSincronizados(int enviados, int recebidos)
        {
            Enviados = enviados;
            Recebidos = recebidos;
        }
    }
}
