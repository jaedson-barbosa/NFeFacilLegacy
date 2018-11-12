using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorFornecedores : BaseBuscador<FornecedorDI>
    {
        public BuscadorFornecedores() : base(DefinicoesPermanentes.ModoBuscaFornecedor)
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterFornecedores().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public FornecedorDI BuscarViaDocumento(string documento)
        {
            return TodosItens.FirstOrDefault(x => x.CNPJ == documento);
        }

        protected override (string, string) ItemComparado(FornecedorDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0: return (item.Nome, null);
                case 1: return (item.CNPJ, null);
                default: return (item.Nome, item.CNPJ);
            }
        }

        protected override void InvalidarItem(FornecedorDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0:
                    item.Nome = InvalidItem;
                    break;
                case 1:
                    item.CNPJ = InvalidItem;
                    break;
                default:
                    item.Nome = item.CNPJ = InvalidItem;
                    break;
            }
        }
    }
}
