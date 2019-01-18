using BaseGeral;
using BaseGeral.ItensBD;
using System.Linq;

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerCompradores : ControllerGerenciadorGeral
    {
        public ControllerCompradores() : base("Inativar", DefinicoesPermanentes.ModoBuscaComprador)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosItens = repo.ObterCompradores()
                    .Select(x => new ExibicaoEspecifica<(string, Comprador)>(x, x.Item1, x.Item2.Telefone, x.Item2.Nome))
                    .ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public override void Adicionar()
        {
            MainPage.Current.Navegar<AdicionarComprador>();
        }

        public override void Editar(ExibicaoGenerica contexto)
        {
            var conj = contexto.Convert<(string, Comprador)>();
            MainPage.Current.Navegar<AdicionarComprador>();
        }

        public override void AcaoSecundaria(ExibicaoGenerica contexto)
        {
            var conj= contexto.Convert<(string, Comprador)>();

            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(conj.Item2, DefinicoesTemporarias.DateTimeNow);
                Remover(contexto);
            }
        }

        protected override (string, string) ItemComparado(ExibicaoGenerica item, int modoBusca)
        {
            var conj = item.Convert<(string, Comprador)>();
            switch (modoBusca)
            {
                case 0: return (conj.Item2.Nome, null);
                case 1: return (conj.Item1, null);
                default: return (conj.Item2.Nome, conj.Item1);
            }
        }

        protected override void InvalidarItem(ExibicaoGenerica item, int modoBusca)
        {
            var conj = item.Convert<(string, Comprador)>();
            switch (modoBusca)
            {
                case 0:
                    conj.Item2.Nome = InvalidItem;
                    break;
                case 1:
                    conj.Item1 = InvalidItem;
                    break;
                default:
                    conj.Item2.Nome = conj.Item1 = InvalidItem;
                    break;
            }
        }
    }
}
