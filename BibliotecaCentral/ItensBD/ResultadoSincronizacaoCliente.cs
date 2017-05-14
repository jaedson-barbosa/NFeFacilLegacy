using System;

namespace BibliotecaCentral.ItensBD
{
    public sealed class ResultadoSincronizacaoCliente
    {
        public int Id { get; set; }
        public DateTime MomentoSincronizacao { get; set; }
        public int NumeroNotasTrafegadas { get; set; }
        public int NumeroDadosBaseTrafegados { get; set; }
        public bool SincronizacaoAutomatica { get; set; }
    }
}
