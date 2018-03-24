using static BaseGeral.Sincronizacao.ConfiguracoesSincronizacao;
using System;
using BaseGeral.Sincronizacao.FastServer;

namespace BaseGeral.Sincronizacao.Servidor
{
    internal sealed class ControllerInformacoes
    {
        [UriFormat("/BrechaSeguranca/{senha}")]
        public RestResponse BrechaSeguranca(int senha)
        {
            if (GerenciadorServidor.Current.BrechaAberta)
            {
                if (senha != SenhaTemporária)
                {
                    return new RestResponse
                    {
                        Sucesso = false,
                        ContentData = "A senha informada não está certa."
                    };
                }

                return new RestResponse
                {
                    Sucesso = true,
                    ContentData = SenhaPermanente.ToString()
                };
            }
            else
            {
                return new RestResponse
                {
                    Sucesso = false,
                    ContentData = "Novas conexões não são aceitas agora."
                };
            }
        }
    }
}
