using BaseGeral.ItensBD;
using System;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorProduto : BaseBuscador<ProdutoDI>
    {
        public Func<ProdutoDI, bool> ValidacaoAdicional { get; set; }

        public BuscadorProduto() : base(DefinicoesPermanentes.ModoBuscaProduto)
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterProdutosOrdenados().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        protected override (string, string) ItemComparado(ProdutoDI item, int modoBusca)
        {
            if (!ValidacaoAdicional?.Invoke(item) ?? false)
                return (InvalidItem, null);
            switch (modoBusca)
            {
                case 0: return (item.Descricao, null);
                case 1: return (item.CodigoProduto, null);
                default: return (item.Descricao, item.CodigoProduto);
            }
        }

        protected override void InvalidarItem(ProdutoDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0:
                    item.Descricao = InvalidItem;
                    break;
                case 1:
                    item.CodigoProduto = InvalidItem;
                    break;
                default:
                    item.Descricao = item.CodigoProduto = InvalidItem;
                    break;
            }
        }
    }
}
