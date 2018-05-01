using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorCliente : BaseBuscador<ClienteDI>
    {
        public BuscadorCliente()
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterClientes().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        protected override string ItemComparado(ClienteDI item, int modoBusca) =>
            DefinicoesPermanentes.ModoBuscaCliente == 0 ? item.Nome : item.Documento;

        protected override void InvalidarItem(ClienteDI item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaProduto == 0) item.Nome = InvalidProduct;
            else item.CPF = item.CNPJ = item.IdEstrangeiro = InvalidProduct;
        }
    }
}
