using Newtonsoft.Json;
using NFeFacil.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace NFeFacil.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacao
    {
        [UriFormat("/Sincronizar/{senha}/{minimo}")]
        public IGetResponse Sincronizar(int senha, long minimo, [FromContent] ConjuntoBanco pacote)
        {
            if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                throw new SenhaErrada(senha);

            try
            {
                pacote.AnalisarESalvar();

                var retorno = new ConjuntoBanco(pacote, DateTime.FromBinary(minimo));
                return new GetResponse(GetResponse.ResponseStatus.OK, retorno);
            }
            catch (Exception e)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, e);
            }
        }
    }
}
