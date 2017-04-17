using static BibliotecaCentral.Configuracoes.ConfiguracoesSincronizacao;
using System.Threading.Tasks;
using System.Linq;
using BibliotecaCentral.Sincronizacao.Pacotes;

namespace BibliotecaCentral.Sincronizacao.Cliente
{
    internal sealed class ClienteSincronizacaoNotas : ConexaoComServidor
    {
        public async Task<ItensSincronizados> Sincronizar()
        {
            var envio = await ProcessamentoNotas.ObterAsync();
            await EnviarAsync(envio);
            var receb = await ReceberAsync();
            await ProcessamentoNotas.SalvarAsync(receb);
            return new ItensSincronizados(envio.XMLs.Count(), receb.XMLs.Count());
        }

        private async Task EnviarAsync(NotasFiscais notas)
        {
            await SendRequest<string>("Notas", Método.POST, SenhaPermanente, notas);
        }

        private async Task<NotasFiscais> ReceberAsync()
        {
            return await SendRequest<NotasFiscais>("Notas", Método.GET, SenhaPermanente, null).ConfigureAwait(false);
        }
    }
}
