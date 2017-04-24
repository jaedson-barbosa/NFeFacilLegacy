using System;

namespace NFeFacil.ItensBD
{
    public sealed class ResultadoSincronizacaoServidor
    {
        public int Id { get; set; }
        public DateTime MomentoRequisicao { get; set; }
        public int TipoDadoSolicitado { get; set; }
        public bool SucessoSolicitacao { get; set; }
    }
}
