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
        [UriFormat("/Dados/POST/{senha}")]
        public IPostResponse ClienteServidorAsync(int senha, [FromContent] DadosBase pacote)
        {
            return SupervisionarOperacao.Iniciar(() =>
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);
                ProcessamentoDadosBase.Salvar(pacote);
                return new PostResponse(PostResponse.ResponseStatus.Created);
            }, pacote.HoraRequisição, TipoDado.DadoBase);
        }

        [UriFormat("/Dados/GET/{senha}")]
        public IGetResponse ServidorCliente(int senha)
        {
            return SupervisionarOperacao.Iniciar(() =>
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);
                return new GetResponse(GetResponse.ResponseStatus.OK, ProcessamentoDadosBase.Obter());
            }, DateTime.Now, TipoDado.DadoBase);
        }
    }
}
