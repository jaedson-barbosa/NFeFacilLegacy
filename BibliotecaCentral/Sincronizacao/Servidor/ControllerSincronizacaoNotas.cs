using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Threading.Tasks;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoNotas
    {
        [UriFormat("/Notas/POST/{senha}")]
        public async Task<IPostResponse> ClienteServidor(int senha, [FromContent] NotasFiscais pacote)
        {
            return await SupervisionarOperacao.Iniciar(async () =>
            {
                if (senha != Configuracoes.ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);
                await ProcessamentoNotas.SalvarAsync(pacote);
                return new PostResponse(PostResponse.ResponseStatus.Created);
            }, pacote.HoraRequisição, TipoDado.NotaFiscal);
        }

        [UriFormat("/Notas/GET/{senha}")]
        public async Task<IGetResponse> ServidorCliente(int senha)
        {
            return await SupervisionarOperacao.Iniciar(async () =>
            {
                if (senha != Configuracoes.ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);
                return new GetResponse(GetResponse.ResponseStatus.OK, await ProcessamentoNotas.ObterAsync());
            }, DateTime.Now, TipoDado.NotaFiscal);
        }
    }
}
