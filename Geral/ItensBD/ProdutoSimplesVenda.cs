using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using System;

namespace BaseGeral.ItensBD
{
    public sealed class ProdutoSimplesVenda : IGuidId
    {
        public Guid Id { get; set; }
        public Guid IdBase { get; set; }

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
            IdBase = original.Id;
        }

        public void CalcularTotalLíquido()
        {
            TotalLíquido = ValorUnitario * Quantidade + Frete + Seguro + DespesasExtras - Desconto;
        }

        public ProdutoOuServico ToProdutoOuServico()
        {
            using (var leitura = new Repositorio.Leitura())
            {
                var produtoBase = leitura.ObterProduto(IdBase);
                var produto = produtoBase.ToProdutoOuServico();
                if (produto.ValorUnitarioTributo == produto.ValorUnitario)
                    produto.ValorUnitarioTributo = ValorUnitario;
                produto.ValorUnitario = ValorUnitario;
                produto.QuantidadeComercializada = Quantidade;
                produto.QuantidadeTributada = Quantidade;
                produto.Frete = Frete;
                produto.Seguro = Seguro;
                produto.DespesasAcessorias = DespesasExtras;
                produto.Desconto = Desconto;
                produto.ValorTotal = Quantidade * ValorUnitario;
                return produto;
            }
        }
    }
}
