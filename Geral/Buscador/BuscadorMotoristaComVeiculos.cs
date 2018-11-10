using System.Linq;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorMotoristaComVeiculos : BaseBuscador<MotoristaManipulacaoNFe>
    {
        public BuscadorMotoristaComVeiculos() : base(DefinicoesPermanentes.ModoBuscaMotorista)
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterMotoristasComVeiculos().Select(x => new MotoristaManipulacaoNFe
                {
                    Root = x.Item1,
                    Principal = x.Item2,
                    Secundarios = x.Item3
                }).ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public MotoristaManipulacaoNFe BuscarViaDocumento(string documento)
        {
            return TodosItens.FirstOrDefault(x => x.Root.Documento == documento);
        }

        protected override (string, string) ItemComparado(MotoristaManipulacaoNFe item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0: return (item.Root.Nome, null);
                case 1: return (item.Root.Documento, null);
                default: return (item.Root.Nome, item.Root.Documento);
            }
        }

        protected override void InvalidarItem(MotoristaManipulacaoNFe item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0:
                    item.Root.Nome = InvalidProduct;
                    break;
                case 1:
                    item.Root.CPF = item.Root.CNPJ = InvalidProduct;
                    break;
                default:
                    item.Root.Nome = item.Root.CPF = item.Root.CNPJ = InvalidProduct;
                    break;
            }
        }
    }
}
