using BaseGeral.ItensBD;
using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorComprador : BaseBuscador<ExibicaoComprador>
    {
        public BuscadorComprador() : base(DefinicoesPermanentes.ModoBuscaComprador)
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

        protected override (string, string) ItemComparado(ExibicaoComprador item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0: return (item.Root.Nome, null);
                case 1: return (item.NomeEmpresa, null);
                default: return (item.Root.Nome, item.NomeEmpresa);
            }
        }

        protected override void InvalidarItem(ExibicaoComprador item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0:
                    item.Root.Nome = InvalidProduct;
                    break;
                case 1:
                    item.NomeEmpresa = InvalidProduct;
                    break;
                default:
                    item.Root.Nome = item.NomeEmpresa = InvalidProduct;
                    break;
            }
        }
    }

    public struct ExibicaoComprador
    {
        public Comprador Root { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
