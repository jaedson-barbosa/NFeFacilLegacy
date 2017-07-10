using System;
using System.Collections.Generic;

namespace BibliotecaCentral.ItensBD
{
    public sealed class RegistroVenda
    {
        public Guid Id { get; set; }
        public NFeDI NotaFiscalRelacionada { get; set; }

        public EmitenteDI Emitente { get; set; }
        public Vendedor Vendedor { get; set; }
        public ClienteDI Cliente { get; set; }
        public MotoristaDI Motorista { get; set; }
        public List<ProdutoSimplesVenda> Produtos { get; set; }
        public DateTime DataHoraVenda { get; set; }
        public string Observações { get; set; }
        public double DescontoTotal { get; set; }
    }
}
