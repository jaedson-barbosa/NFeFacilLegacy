using Microsoft.EntityFrameworkCore;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System;
using System.Linq;

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

        public void DesregistrarAlteracaoEstoque(AplicativoContext db)
        {
            var estoque = db.Estoque.Find(IdBase);
            if (estoque != null)
            {
                estoque.UltimaData = DateTime.Now;
                estoque.Alteracoes.Add(new AlteracaoEstoque
                {
                    Alteração = Quantidade
                });
            }
            db.SaveChanges();
        }

        public void RegistrarAlteracaoEstoque(AplicativoContext db)
        {
            var estoque = db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == IdBase);
            if (estoque != null)
            {
                estoque.UltimaData = DateTime.Now;
                var alteracao = estoque.Alteracoes.FirstOrDefault(x => x.Id == Id);
                if (alteracao != null)
                {
                    alteracao.Alteração = Quantidade * -1;
                    db.Update(alteracao);
                }
                else
                {
                    estoque.Alteracoes.Add(new AlteracaoEstoque()
                    {
                        Id = Id,
                        Alteração = Quantidade * -1
                    });
                }
                db.SaveChanges();
            }
        }
    }
}
