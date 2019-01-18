using BaseGeral;
using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using System.Linq;

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerMotoristas : ControllerGerenciadorGeral
    {
        public ControllerMotoristas() : base("Inativar", DefinicoesPermanentes.ModoBuscaMotorista)
        {
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosItens = repo.ObterMotoristas()
                    .Select(x => new ExibicaoEspecifica<MotoristaDI>(x, x.Documento, x.XMun, x.Nome))
                    .ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        public override void Adicionar()
        {
            MainPage.Current.Navegar<AdicionarMotorista>();
        }

        public override void Editar(ExibicaoGenerica contexto)
        {
            MainPage.Current.Navegar<AdicionarMotorista>(contexto.Convert<MotoristaDI>());
        }

        public override void AcaoSecundaria(ExibicaoGenerica contexto)
        {
            var mot = contexto.Convert<MotoristaDI>();
            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(mot, DefinicoesTemporarias.DateTimeNow);
                Remover(contexto);
            }
        }

        protected override (string, string) ItemComparado(ExibicaoGenerica item, int modoBusca)
            => BuscadorMotorista.StaticItemComparado(item.Convert<MotoristaDI>(), modoBusca);

        protected override void InvalidarItem(ExibicaoGenerica item, int modoBusca)
            => BuscadorMotorista.StaticInvalidarItem(item.Convert<MotoristaDI>(), modoBusca);
    }
}
