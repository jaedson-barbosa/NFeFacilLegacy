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

        internal bool ValidarTudo(ILog log, params (bool isErrado, string msg)[] itensExtras)
        {
            if (!ValidarTudo(log)) return false;
            foreach (var (isErrado, msg) in itensExtras)
            {
                if (isErrado)
                {
                    log?.Escrever(TitulosComuns.Atenção, msg);
                    return false;
                }
            }
            return true;
        }
    }
}
