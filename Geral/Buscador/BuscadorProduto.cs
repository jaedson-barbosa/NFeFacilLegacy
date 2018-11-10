using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorProduto : BaseBuscador<ProdutoDI>
    {
        public BuscadorProduto() : base(DefinicoesPermanentes.ModoBuscaProduto)
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterProdutos().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        protected override (string, string) ItemComparado(ProdutoDI item, int modoBusca)
        {
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
                    item.Descricao = InvalidProduct;
                    break;
                case 1:
                    item.CodigoProduto = InvalidProduct;
                    break;
                default:
                    item.Descricao = item.CodigoProduto = InvalidProduct;
                    break;
            }
        }
    }
}
