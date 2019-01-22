using BaseGeral.Buscador;
using BaseGeral.View;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public abstract class ControllerGerenciadorGeral : BaseBuscador<ExibicaoGenerica>
    {
        public string AcaoSecundariaLabel { get; protected set; }

        protected ControllerGerenciadorGeral(string acaoSecundariaLabel, int modoBusca) : base(modoBusca)
        {
            AcaoSecundariaLabel = acaoSecundariaLabel;
        }

        public abstract void Adicionar();
        public abstract void Editar(ExibicaoGenerica contexto);
        public abstract void AcaoSecundaria(ExibicaoGenerica contexto);
    }
}
