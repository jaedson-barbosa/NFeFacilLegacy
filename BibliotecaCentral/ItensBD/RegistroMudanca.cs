using System;

namespace BibliotecaCentral.ItensBD
{
    public sealed class RegistroMudanca
    {
        /// <summary>
        /// Id do objeto modificado
        /// </summary>
        public Guid Id { get; set; }
        public int TipoDadoModificado { get; set; }
        public int TipoOperacaoRealizada { get; set; }
        public DateTime MomentoMudanca { get; set; }
    }

    /// <summary>
    /// Tipo de dado sincronizável modificado
    /// </summary>
    public enum TipoDado : short
    {
        Emitente,
        Cliente,
        Motorista,
        Produto,
        NotaFiscal
    }

    public enum TipoOperacao
    {
        Adicao,
        Edicao,
        Remocao
    }
}
