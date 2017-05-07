using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoDadosBase
    {
        [UriFormat("/Dados/{senha}")]
        public IPostResponse ClienteServidorAsync(int senha, [FromContent] DadosBase pacote)
        {
            return SupervisorOperacao.Supervisionar(() =>
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);
                ProcessamentoDadosBase.Salvar(pacote);
                return new PostResponse(PostResponse.ResponseStatus.Created);
            }, pacote.HoraRequisição, TipoDado.DadoBase);
        }

        [UriFormat("/Dados/{senha}")]
        public IGetResponse ServidorCliente(int senha)
        {
            return SupervisorOperacao.Supervisionar(() =>
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);
                return new GetResponse(GetResponse.ResponseStatus.OK, ProcessamentoDadosBase.Obter());
            }, DateTime.Now, TipoDado.DadoBase);
        }
    }
}
