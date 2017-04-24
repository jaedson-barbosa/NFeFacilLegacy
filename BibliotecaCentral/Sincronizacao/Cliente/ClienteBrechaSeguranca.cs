using static BibliotecaCentral.Configuracoes.ConfiguracoesSincronizacao;
using BibliotecaCentral.Log;
using System.Threading.Tasks;
using BibliotecaCentral.Sincronizacao.Pacotes;

namespace BibliotecaCentral.Sincronizacao.Cliente
{
    public sealed class ClienteBrechaSeguranca : ConexaoComServidor
    {
        private ILog Log;

        public ClienteBrechaSeguranca(ILog log)
        {
            Log = log;
        }

        public async Task EstabelecerConexao(int senha)
        {
            var info = await EnviarRequisição(senha);
            SenhaPermanente = info.Senha;
            Log.Escrever(TitulosComuns.Sucesso, "Chave de segurança decodificada e salva com sucesso.");
        }

        public async Task<InfoSegurancaConexao> EnviarRequisição(int senha)
        {
            return await SendRequest<InfoSegurancaConexao>("BrechaSeguranca", Método.GET, senha, null);
        }
    }
}
