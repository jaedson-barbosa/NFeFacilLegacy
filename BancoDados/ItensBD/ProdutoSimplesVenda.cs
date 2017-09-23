using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System;

namespace NFeFacil.ItensBD
{
    public sealed class ProdutoSimplesVenda
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
            using (var db = new AplicativoContext())
            {
                var ProdutoBase = db.Produtos.Find(IdBase);
                return new ProdutoOuServico
                {
                    CodigoProduto = ProdutoBase.CodigoProduto,
                    CodigoBarras = ProdutoBase.CodigoBarras,
                    Descricao = ProdutoBase.Descricao,
                    NCM = ProdutoBase.NCM,
                    EXTIPI = ProdutoBase.EXTIPI,
                    CFOP = !string.IsNullOrEmpty(ProdutoBase.CFOP) ? int.Parse(ProdutoBase.CFOP) : 0,
                    UnidadeComercializacao = ProdutoBase.UnidadeComercializacao,
                    CodigoBarrasTributo = ProdutoBase.CodigoBarrasTributo,
                    UnidadeTributacao = ProdutoBase.UnidadeTributacao,
                    ValorUnitarioTributo = ProdutoBase.ValorUnitarioTributo,
                    ValorUnitario = ProdutoBase.ValorUnitario,
                    QuantidadeComercializada = Quantidade,
                    QuantidadeTributada = Quantidade,
                    Frete = Frete != 0 ? Frete.ToString("0.00") : null,
                    Seguro = Seguro != 0 ? Seguro.ToString("0.00") : null,
                    DespesasAcessórias = DespesasExtras != 0 ? DespesasExtras.ToString("0.00") : null,
                    Desconto = Desconto != 0 ? Desconto.ToString("0.00") : null,
                    ValorTotal = Quantidade * ProdutoBase.ValorUnitario
                };
            }
        }
    }
}
