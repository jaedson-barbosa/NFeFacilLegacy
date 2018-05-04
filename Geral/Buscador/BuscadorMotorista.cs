using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorMotorista : BaseBuscador<MotoristaDI>
    {
        public BuscadorMotorista()
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterMotoristas().ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public MotoristaDI BuscarViaDocumento(string documento)
        {
            return TodosItens.FirstOrDefault(x => x.Documento == documento);
        }

        protected override (string, string) ItemComparado(MotoristaDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0: return (item.Nome, null);
                case 1: return (item.Documento, null);
                default: return (item.Nome, item.Documento);
            }
        }

        protected override void InvalidarItem(MotoristaDI item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0:
                    item.Nome = InvalidProduct;
                    break;
                case 1:
                    item.CPF = item.CNPJ = InvalidProduct;
                    break;
                default:
                    item.Nome = item.CPF = item.CNPJ = InvalidProduct;
                    break;
            }
        }
    }
}
