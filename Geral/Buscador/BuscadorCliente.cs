using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorCliente : BaseBuscador<ClienteDI>
    {
        public BuscadorCliente() : base(DefinicoesPermanentes.ModoBuscaCliente)
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterClientes().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public ClienteDI BuscarViaDocumento(string documento)
        {
            return TodosItens.FirstOrDefault(x => x.Documento == documento);
        }

        protected override (string, string) ItemComparado(ClienteDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0: return (item.Nome, null);
                case 1: return (item.Documento, null);
                default: return (item.Nome, item.Documento);
            }
        }

        protected override void InvalidarItem(ClienteDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0:
                    item.Nome = InvalidProduct;
                    break;
                case 1:
                    item.CPF = item.CNPJ = item.IdEstrangeiro = InvalidProduct;
                    break;
                default:
                    item.Nome = item.CPF = item.CNPJ = item.IdEstrangeiro = InvalidProduct;
                    break;
            }
        }
    }
}
