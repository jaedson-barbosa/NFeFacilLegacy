using System;

namespace BibliotecaCentral.ItensBD
{
    public sealed class AlteracaoEstoque
    {
        public DateTime Id { get; set; }

        public ProdutoDI Produto { get; set; }
        public double Alteração { get; set; }
    }
}
