using NFeFacil.Sincronizacao.Pacotes;
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
        public IGetResponse SincronizarDadosBase(int senha, long minimo, [FromContent] ConjuntoBanco pacote)
        {
            if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                throw new SenhaErrada(senha);

            try
            {
                DateTime atual = DateTime.Now;
                pacote.InstanteSincronizacao = atual;
                pacote.AnalisarESalvar();

                var retorno = new ConjuntoBanco(pacote, DateTime.FromBinary(minimo), atual);
                return new GetResponse(GetResponse.ResponseStatus.OK, retorno);
            }
            catch (Exception e)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, e);
            }
        }

        [UriFormat("/SincronizarNotasFiscais/{senha}/{minimo}")]
        public IGetResponse SincronizarNotasFiscais(int senha, long minimo, [FromContent] ConjuntoNotasFiscais pacote)
        {
            if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                throw new SenhaErrada(senha);

            try
            {
                DateTime atual = DateTime.Now;
                pacote.InstanteSincronizacao = atual;
                pacote.AnalisarESalvar();

                var retorno = new ConjuntoNotasFiscais(pacote, DateTime.FromBinary(minimo), atual);
                return new GetResponse(GetResponse.ResponseStatus.OK, retorno);
            }
            catch (Exception e)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, e);
            }
        }
    }
}
