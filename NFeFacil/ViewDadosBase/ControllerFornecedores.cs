using BaseGeral;
using BaseGeral.ItensBD;
using System.Linq;

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerFornecedores : ControllerGerenciadorGeral
    {
        public ControllerFornecedores() : base("Em breve...", DefinicoesPermanentes.ModoBuscaFornecedor)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosItens = repo.ObterFornecedores()
                    .Select(x => new ExibicaoEspecifica<FornecedorDI>(x, x.CNPJ, x.NomeMunicipio, x.Nome))
                    .ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public override void Adicionar()
        {
            MainPage.Current.Navegar<AdicionarFornecedor>();
        }

        public override void Editar(ExibicaoGenerica contexto)
        {
            var fornecedor = contexto.Convert<FornecedorDI>();
            MainPage.Current.Navegar<AdicionarFornecedor>(fornecedor);
        }

        public override void AcaoSecundaria(ExibicaoGenerica contexto) { }

        protected override (string, string) ItemComparado(ExibicaoGenerica item, int modoBusca)
        {
            var forn = item.Convert<FornecedorDI>();
            switch (modoBusca)
            {
                case 0: return (forn.Nome, null);
                case 1: return (forn.CNPJ, null);
                default: return (forn.Nome, forn.CNPJ);
            }
        }

        protected override void InvalidarItem(ExibicaoGenerica item, int modoBusca)
        {
            var forn = item.Convert<FornecedorDI>();
            switch (modoBusca)
            {
                case 0:
                    forn.Nome = InvalidItem;
                    break;
                case 1:
                    forn.CNPJ = InvalidItem;
                    break;
                default:
                    forn.Nome = forn.CNPJ = InvalidItem;
                    break;
            }
        }
    }
}
