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

        protected override string ItemComparado(MotoristaDI item, int modoBusca) =>
            DefinicoesPermanentes.ModoBuscaMotorista == 0 ? item.Nome : item.Documento;

        protected override void InvalidarItem(MotoristaDI item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaMotorista == 0) item.Nome = InvalidProduct;
            else item.CPF = item.CNPJ = InvalidProduct;
        }
    }

    public sealed class BuscadorMotoristaComVeiculos : BaseBuscador<MotoristaManipulacaoNFe>
    {
        public BuscadorMotoristaComVeiculos()
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

        protected override string ItemComparado(MotoristaManipulacaoNFe item, int modoBusca) =>
            DefinicoesPermanentes.ModoBuscaMotorista == 0 ? item.Root.Nome : item.Root.Documento;

        protected override void InvalidarItem(MotoristaManipulacaoNFe item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaMotorista == 0) item.Root.Nome = InvalidProduct;
            else item.Root.CPF = item.Root.CNPJ = InvalidProduct;
        }
    }
}
