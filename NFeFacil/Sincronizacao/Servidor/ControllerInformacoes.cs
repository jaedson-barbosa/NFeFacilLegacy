using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
using System;
using NFeFacil.Sincronizacao.FastServer;

namespace NFeFacil.Sincronizacao.Servidor
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
