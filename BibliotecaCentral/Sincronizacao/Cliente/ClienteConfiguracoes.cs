using static BibliotecaCentral.Configuracoes.ConfiguracoesSincronizacao;
using System.Threading.Tasks;

namespace BibliotecaCentral.Sincronizacao.Cliente
{
    internal sealed class ClienteConfiguracoes : ConexaoComServidor
    {
        public async Task<Pacotes.ConfiguracoesServidor> ObterConfiguracoes()
        {
            return await SendRequest<Pacotes.ConfiguracoesServidor>($"Configuracoes", Método.GET, SenhaPermanente, null);
        }
    }
}
