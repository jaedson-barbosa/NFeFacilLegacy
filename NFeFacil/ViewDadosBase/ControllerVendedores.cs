using BaseGeral;
using BaseGeral.ItensBD;
using System.Linq;

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerVendedores : ControllerGerenciadorGeral
    {
        public ControllerVendedores() : base("Inativar", DefinicoesPermanentes.ModoBuscaVendedor)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosItens = repo.ObterVendedores()
                    .Select(x => new ExibicaoEspecifica<Vendedor>(
                        x,
                        ExtensoesPrincipal.AplicarMáscaraDocumento(x.CPFStr),
                        null, x.Nome))
                    .ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public override void Adicionar()
        {
            MainPage.Current.Navegar<AdicionarVendedor>();
        }

        public override void Editar(ExibicaoGenerica contexto)
        {
            var vend = contexto.Convert<Vendedor>();
            MainPage.Current.Navegar<AdicionarVendedor>(vend);
        }

        public override void AcaoSecundaria(ExibicaoGenerica contexto)
        {
            var vend = contexto.Convert<Vendedor>();
            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(vend, DefinicoesTemporarias.DateTimeNow);
                Remover(contexto);
            }
        }

        protected override (string, string) ItemComparado(ExibicaoGenerica item, int modoBusca)
        {
            var vend = item.Convert<Vendedor>();
            switch (modoBusca)
            {
                case 0: return (vend.Nome, null);
                case 1: return (vend.CPFStr, null);
                default: return (vend.Nome, vend.CPFStr);
            }
        }

        protected override void InvalidarItem(ExibicaoGenerica item, int modoBusca)
        {
            var vend = item.Convert<Vendedor>();
            switch (modoBusca)
            {
                case 0:
                    vend.Nome = InvalidItem;
                    break;
                case 1:
                    vend.CPFStr = InvalidItem;
                    break;
                default:
                    vend.Nome = vend.CPFStr = InvalidItem;
                    break;
            }
        }
    }
}
