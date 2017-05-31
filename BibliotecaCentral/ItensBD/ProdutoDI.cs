using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System;

namespace BibliotecaCentral.ItensBD
{
    public sealed class ProdutoDI
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CodigoProduto { get; set; }
        public string CodigoBarras { get; set; } = "";
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public string EXTIPI { get; set; }
        public string CFOP { get; set; }
        public string UnidadeComercializacao { get; set; }
        public double ValorUnitario { get; set; }
        public string CodigoBarrasTributo { get; set; } = "";
        public string UnidadeTributacao { get; set; }
        public double ValorUnitarioTributo { get; set; }

        public ProdutoDI() { }
        public ProdutoDI(ProdutoOuServico other)
        {
            CodigoProduto = other.CodigoProduto;
            CodigoBarras = other.CodigoBarras;
            Descricao = other.Descricao;
            NCM = other.NCM;
            EXTIPI = other.EXTIPI;
            CFOP = other.CFOP;
            UnidadeComercializacao = other.UnidadeComercializacao;
            ValorUnitario = other.ValorUnitario;
            CodigoBarrasTributo = other.CodigoBarrasTributo;
            UnidadeTributacao = other.UnidadeTributacao;
            ValorUnitarioTributo = other.ValorUnitarioTributo;
        }

        public ProdutoOuServico ToProdutoOuServico()
        {
            return new ProdutoOuServico
            {
                CodigoProduto = CodigoProduto,
                CodigoBarras = CodigoBarras,
                Descricao = Descricao,
                NCM = NCM,
                EXTIPI = EXTIPI,
                CFOP = CFOP,
                UnidadeComercializacao = UnidadeComercializacao,
                ValorUnitario = ValorUnitario,
                CodigoBarrasTributo = CodigoBarrasTributo,
                UnidadeTributacao = UnidadeTributacao,
                ValorUnitarioTributo = ValorUnitarioTributo
            };
        }
    }
}
