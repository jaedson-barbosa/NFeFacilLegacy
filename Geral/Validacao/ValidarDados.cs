using BaseGeral.Log;

namespace BaseGeral.Validacao
{
    public sealed class ValidarDados
    {
        private IValidavel[] itens;

        public ValidarDados(params IValidavel[] conjuntosItens)
        {
            itens = conjuntosItens;
        }

        public bool ValidarTudo(bool exibirMensagem, params (bool isErrado, string msg)[] itensExtras)
        {
            for (int i = 0; i < itens.Length; i++)
            {
                if (!itens[i].Validar(exibirMensagem)) return false;
            }

            var log = exibirMensagem ? Popup.Current : null;
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
