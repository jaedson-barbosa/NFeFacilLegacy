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
        [UriFormat("/Sincronizar/{senha}")]
        public IGetResponse Sincronizar(int senha, [FromContent] ConjuntoBanco pacote)
        {
            if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                throw new SenhaErrada(senha);

            using (var db = new AplicativoContext())
            {
                pacote.AnalisarESalvar(db);
                db.SaveChanges();

                var retorno = new ConjuntoBanco(pacote, db);
                return new GetResponse(GetResponse.ResponseStatus.OK, retorno);
            }
        }
    }
}
