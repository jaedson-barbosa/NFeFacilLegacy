using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorProduto : BaseBuscador<ProdutoDI>
    {
        public BuscadorProduto()
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterProdutos().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        protected override string ItemComparado(ProdutoDI item, int modoBusca) =>
            DefinicoesPermanentes.ModoBuscaProduto == 0 ? item.Descricao : item.CodigoProduto;

        protected override void InvalidarItem(ProdutoDI item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaProduto == 0) item.Descricao = InvalidProduct;
            else item.CodigoProduto = InvalidProduct;
        }
    }
}
