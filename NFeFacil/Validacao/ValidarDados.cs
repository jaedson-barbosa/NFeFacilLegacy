using NFeFacil.Log;

namespace NFeFacil.Validacao
{
    public sealed class ValidarDados
    {
        private IValidavel[] itens;

        public ValidarDados(params IValidavel[] conjuntosItens)
        {
            itens = conjuntosItens;
        }

        public bool ValidarTudo(ILog log)
        {
            foreach (var item in itens)
            {
                if (!item.Validar(log)) return false;
            }
            return true;
        }

        internal bool ValidarTudo(ILog log, params ConjuntoAnalise[] itensExtras)
        {
            if (!ValidarTudo(log)) return false;
            foreach (var item in itensExtras)
            {
                if (item.EstáErrado)
                {
                    log.Escrever(TitulosComuns.ErroSimples, item.Mensagem);
                    return false;
                }
            }
            return true;
        }
    }
}
