using System;
using System.Collections.Generic;

namespace BaseGeral.ItensBD
{
    public sealed class RegistroVenda : IUltimaData, IGuidId
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }
        public string NotaFiscalRelacionada { get; set; }

        public Guid Emitente { get; set; }
        public Guid Vendedor { get; set; }
        public Guid Cliente { get; set; }
        public Guid Motorista { get; set; }
        public List<ProdutoSimplesVenda> Produtos { get; set; }
        public DateTime DataHoraVenda { get; set; }
        public string Observações { get; set; }
        public double DescontoTotal { get; set; }
        public bool Cancelado { get; set; }

        public string TipoFrete { get; set; }
        public DateTime PrazoEntrega { get; set; }
        public string PrazoPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public Guid Comprador { get; set; }

        public string MotivoEdicao { get; set; }
    }
}
