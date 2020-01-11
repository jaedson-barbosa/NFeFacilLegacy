using System;

namespace BaseGeral.ItensBD
{
    public sealed class CancelamentoRegistroVenda : IGuidId
    {
        public Guid Id { get; set; }
        public DateTime MomentoCancelamento { get; set; }
        public string Motivo { get; set; }
    }
}
