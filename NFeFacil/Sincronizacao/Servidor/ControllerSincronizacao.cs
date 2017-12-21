using NFeFacil.PacotesBanco;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace NFeFacil.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.Singleton)]
    internal sealed class ControllerSincronizacao
    {
        [UriFormat("/SincronizarDadosBase/{senha}/{minimo}")]
        public IGetResponse SincronizarDadosBase(int senha, long minimo, [FromContent] ConjuntoDadosBase pacote)
        {
            try
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                {
                    throw new SenhaErrada(senha);
                }

                DateTime atual = DefinicoesTemporarias.DateTimeNow;
                pacote.InstanteSincronizacao = atual;
                DateTime minimoProcessado = DateTime.FromBinary(minimo);
                pacote.AnalisarESalvar(minimoProcessado);

                var retorno = new ConjuntoDadosBase(pacote, minimoProcessado, atual);
                return new GetResponse(GetResponse.ResponseStatus.OK, retorno);
            }
            catch (Exception e)
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound, e);
            }
        }

        [UriFormat("/SincronizarNotasFiscais/{senha}/{minimo}")]
        public IGetResponse SincronizarNotasFiscais(int senha, long minimo, [FromContent] ConjuntoNotasFiscais pacote)
        {
            try
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                {
                    throw new SenhaErrada(senha);
                }

                DateTime atual = DefinicoesTemporarias.DateTimeNow;
                pacote.InstanteSincronizacao = atual;
                pacote.AnalisarESalvar();

                var retorno = new ConjuntoNotasFiscais(pacote, DateTime.FromBinary(minimo), atual);
                return new GetResponse(GetResponse.ResponseStatus.OK, retorno);
            }
            catch (Exception e)
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound, e);
            }
        }
    }
}
