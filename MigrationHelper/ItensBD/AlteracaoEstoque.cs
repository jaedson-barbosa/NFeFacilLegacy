using System;

namespace BaseGeral.ItensBD
{
    public sealed class AlteracaoEstoque : IGuidId
    {
        public Guid Id { get; set; }
        public double Alteração { get; set; }
        public DateTime MomentoRegistro { get; set; }
    }
}
