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

        protected override string ItemComparado(MotoristaDI item, int modoBusca) =>
            DefinicoesPermanentes.ModoBuscaMotorista == 0 ? item.Nome : item.Documento;

        protected override void InvalidarItem(MotoristaDI item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaMotorista == 0) item.Nome = InvalidProduct;
            else item.CPF = item.CNPJ = InvalidProduct;
        }
    }
}
