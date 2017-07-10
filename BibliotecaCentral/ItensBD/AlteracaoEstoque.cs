using System;

namespace BibliotecaCentral.ItensBD
{
    public sealed class AlteracaoEstoque
    {
        public DateTime Id { get; set; }
        public Guid RegistroVendaRelacionado { get; set; }
        public Guid ProdutoRelacionado { get; set; }
        public double Alteração { get; set; }
    }
}
