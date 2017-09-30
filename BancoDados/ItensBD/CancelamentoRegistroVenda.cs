using System;

namespace NFeFacil.ItensBD
{
    public sealed class CancelamentoRegistroVenda
    {
        public Guid Id { get; set; }
        public DateTime MomentoCancelamento { get; set; }
        public string Motivo { get; set; }
    }
}
