using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorComprador : BaseBuscador<ExibicaoComprador>
    {
        public BuscadorComprador()
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterCompradores().Select(x => new ExibicaoComprador
                {
                    Root = x.Item2,
                    NomeEmpresa = x.Item1
                }).ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        protected override string ItemComparado(ExibicaoComprador item, int modoBusca) =>
            DefinicoesPermanentes.ModoBuscaComprador == 0 ? item.Root.Nome : item.NomeEmpresa;

        protected override void InvalidarItem(ExibicaoComprador item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaComprador == 0) item.Root.Nome = InvalidProduct;
            else item.NomeEmpresa = InvalidProduct;
        }
    }

    public struct ExibicaoComprador
    {
        public Comprador Root { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
