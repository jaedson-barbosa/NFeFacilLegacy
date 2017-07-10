using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace BibliotecaCentral.ItensBD
{
    public sealed class ProdutoSimplesVenda
    {
        public int Id { get; set; }
        public ProdutoDI ProdutoBase { get; set; }

        public string Nome { get; set; }
        public string Unidade { get; set; }
        public double ValorUnitario { get; set; }

        public double Quantidade { get; set; }

        public double Frete { get; set; }
        public double Seguro { get; set; }
        public double DespesasExtras { get; set; }
        public double Desconto { get; set; }

        public double TotalLíquido { get; set; }

        public ProdutoSimplesVenda() { }
        public ProdutoSimplesVenda(ProdutoDI original)
        {
            ProdutoBase = original;
            Nome = original.Descricao;
            Unidade = original.UnidadeComercializacao;
            ValorUnitario = original.ValorUnitario;
        }

        public ProdutoOuServico ToProdutoOuServico()
        {
            using (var db = new AplicativoContext())
            {
                var produtoBase = db.Produtos.Find(ProdutoBase);
                return new ProdutoOuServico
                {
                    CodigoProduto = produtoBase.CodigoProduto,
                    CodigoBarras = produtoBase.CodigoBarras,
                    Descricao = produtoBase.Descricao,
                    NCM = produtoBase.NCM,
                    EXTIPI = produtoBase.EXTIPI,
                    CFOP = !string.IsNullOrEmpty(produtoBase.CFOP) ? int.Parse(produtoBase.CFOP) : 0,
                    UnidadeComercializacao = produtoBase.UnidadeComercializacao,
                    CodigoBarrasTributo = produtoBase.CodigoBarrasTributo,
                    UnidadeTributacao = produtoBase.UnidadeTributacao,
                    ValorUnitarioTributo = produtoBase.ValorUnitarioTributo,
                    ValorUnitario = ValorUnitario,
                    QuantidadeComercializada = Quantidade,
                    QuantidadeTributada = Quantidade,
                    Frete = Frete != 0 ? Frete.ToString("0.00") : null,
                    Seguro = Seguro != 0 ? Seguro.ToString("0.00") : null,
                    DespesasAcessórias = DespesasExtras != 0 ? DespesasExtras.ToString("0.00") : null,
                    Desconto = Desconto != 0 ? Desconto.ToString("0.00") : null,
                    ValorTotal = Quantidade * ValorUnitario
                };
            }
        }
    }
}
