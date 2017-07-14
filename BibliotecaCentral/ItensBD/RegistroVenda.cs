using System;
using System.Collections.Generic;

namespace BibliotecaCentral.ItensBD
{
    public sealed class RegistroVenda
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }
        public Guid NotaFiscalRelacionada { get; set; }

        public Guid Emitente { get; set; }
        public Guid Vendedor { get; set; }
        public Guid Cliente { get; set; }
        public Guid Motorista { get; set; }
        public List<ProdutoSimplesVenda> Produtos { get; set; }
        public DateTime DataHoraVenda { get; set; }
        public string Observações { get; set; }
        public double DescontoTotal { get; set; }
    }
}
