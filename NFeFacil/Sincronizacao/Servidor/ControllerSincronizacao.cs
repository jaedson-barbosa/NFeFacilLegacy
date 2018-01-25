using NFeFacil.Sincronizacao.FastServer;
using NFeFacil.Sincronizacao.Pacotes;
using System;
using System.Xml.Linq;

namespace NFeFacil.Sincronizacao.Servidor
{
    internal sealed class ControllerSincronizacao
    {
        [UriFormat("/SincronizarDadosBase/{senha}/{minimo}")]
        public RestResponse SincronizarDadosBase(int senha, long minimo, [FromContent] ConjuntoDadosBase pacote)
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
                DateTime minimoProcessado = DateTime.FromBinary(minimo);
                pacote.AnalisarESalvar(minimoProcessado);

                var retorno = new ConjuntoDadosBase(pacote, minimoProcessado, atual);
                return new RestResponse
                {
                    Sucesso = true,
                    ContentData = retorno.ToXElement().ToString(SaveOptions.DisableFormatting)
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

        [UriFormat("/SincronizarNotasFiscais/{senha}/{minimo}")]
        public RestResponse SincronizarNotasFiscais(int senha, long minimo, [FromContent] ConjuntoNotasFiscais pacote)
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

                var retorno = new ConjuntoNotasFiscais(pacote, DateTime.FromBinary(minimo), atual);
                return new RestResponse
                {
                    Sucesso = true,
                    ContentData = retorno.ToXElement().ToString(SaveOptions.DisableFormatting)
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
