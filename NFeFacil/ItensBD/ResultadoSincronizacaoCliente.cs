using System;

namespace NFeFacil.ItensBD
{
    public sealed class ResultadoSincronizacaoCliente
    {
        public int Id { get; set; }
        public DateTime MomentoSincronizacao { get; set; }
        public int NumeroNotasEnviadas { get; set; }
        public int NumeroNotasRecebidas { get; set; }
        public int NumeroDadosEnviados { get; set; }
        public int NumeroDadosRecebidos { get; set; }
        public bool SincronizacaoAutomatica { get; set; }
        public bool PodeSincronizarNota { get; set; }
        public bool PodeSincronizarDadoBase { get; set; }
    }
}
