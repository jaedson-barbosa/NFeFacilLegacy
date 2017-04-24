using static NFeFacil.Configuracoes.ConfiguracoesSincronizacao;
using NFeFacil.Log;
using System.Threading.Tasks;
using NFeFacil.Sincronizacao.Pacotes;

namespace NFeFacil.Sincronizacao.Cliente
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
