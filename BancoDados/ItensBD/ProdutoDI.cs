using System;

namespace Banco.ItensBD
{
    public sealed class ProdutoDI
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CodigoProduto { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public string EXTIPI { get; set; }
        public string CFOP { get; set; }
        public string UnidadeComercializacao { get; set; }
        public double ValorUnitario { get; set; }
        public string CodigoBarrasTributo { get; set; } = string.Empty;
        public string UnidadeTributacao { get; set; }
        public double ValorUnitarioTributo { get; set; }
    }
}
