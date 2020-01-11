using BaseGeral.Sincronizacao.FastServer;
using BaseGeral.Sincronizacao.Pacotes;
using System;
using System.Xml.Linq;

namespace BaseGeral.Sincronizacao.Servidor
{
    internal sealed class ControllerSincronizacao
    {
        [UriFormat("/SincronizarDadosBase/{senha}")]
        public RestResponse SincronizarDadosBase(int senha, [FromContent] ConjuntoDadosBase pacote)
        {
            try
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                {
                    return new RestResponse
                    {
                        Sucesso = false,
                        ContentData = "A senha informada não está certa."
                    };
                }

                DateTime atual = DefinicoesTemporarias.DateTimeNow;
                pacote.InstanteSincronizacao = atual;
                pacote.AnalisarESalvar();

                var retorno = new ConjuntoDadosBase(pacote, atual);
                return new RestResponse
                {
                    Sucesso = true,
                    ContentData = retorno.ToXElement<ConjuntoDadosBase>().ToString(SaveOptions.DisableFormatting)
                };
            }
            catch (Exception e)
            {
                return new RestResponse
                {
                    Sucesso = false,
                    ContentData = e.Message
                };
            }
        }

        [UriFormat("/SincronizarNotasFiscais/{senha}")]
        public RestResponse SincronizarNotasFiscais(int senha, [FromContent] ConjuntoNotasFiscais pacote)
        {
            try
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                {
                    return new RestResponse
                    {
                        Sucesso = false,
                        ContentData = "A senha informada não está certa."
                    };
                }

                DateTime atual = DefinicoesTemporarias.DateTimeNow;
                pacote.InstanteSincronizacao = atual;
                pacote.AnalisarESalvar();

                var retorno = new ConjuntoNotasFiscais(pacote, atual);
                return new RestResponse
                {
                    Sucesso = true,
                    ContentData = retorno.ToXElement<ConjuntoNotasFiscais>().ToString(SaveOptions.DisableFormatting)
                };
            }
            catch (Exception e)
            {
                return new RestResponse
                {
                    Sucesso = false,
                    ContentData = e.Message
                };
            }
        }
    }
}
