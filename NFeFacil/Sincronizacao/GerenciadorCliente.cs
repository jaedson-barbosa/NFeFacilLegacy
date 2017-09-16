using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
using NFeFacil.Log;
using NFeFacil.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace NFeFacil.Sincronizacao
{
    public sealed class GerenciadorCliente
    {
        private ILog Log { get; }

        public GerenciadorCliente(ILog log)
        {
            Log = log;
        }

        public async Task EstabelecerConexao(int senha)
        {
            var info = await EnviarAsync<InfoSegurancaConexao>("BrechaSeguranca", HttpMethod.Get, senha, null);
            SenhaPermanente = info.Senha;
            Log.Escrever(TitulosComuns.Sucesso, "Chave de segurança decodificada e salva com sucesso.");
        }

        public async Task Sincronizar()
        {
            using (var db = new AplicativoContext())
            {
                var momento = UltimaSincronizacao;
                var receb = await EnviarAsync<ConjuntoBanco>($"SincronizacaoSimples", HttpMethod.Get, SenhaPermanente, null, momento.ToBinary().ToString());

                var envio = new ConjuntoBanco(receb, db);
                await EnviarAsync<string>("SincronizacaoSimples", HttpMethod.Post, SenhaPermanente, envio);

                Log.Escrever(TitulosComuns.Sucesso, "Sincronização simples concluida.");
                db.SaveChanges();
            }
        }

        public async Task SincronizarTudo()
        {
            using (var db = new AplicativoContext())
            {
                var receb = await EnviarAsync<ConjuntoBanco>($"SincronizacaoSimples", HttpMethod.Get, SenhaPermanente, null, momento.ToBinary().ToString());

                var envio = new ConjuntoBanco(receb, db);
                await EnviarAsync<string>("SincronizacaoSimples", HttpMethod.Post, SenhaPermanente, envio);

                Log.Escrever(TitulosComuns.Sucesso, "Sincronização completa concluida.");
                db.SaveChanges();
            }
        }

        async Task<T> EnviarAsync<T>(string nomeMetodo, HttpMethod metodo, int senha, IPacote corpo, string parametro = null)
        {
            string caminho = $"http://{IPServidor}:8080/{nomeMetodo}/{senha}";
            if (!string.IsNullOrEmpty(parametro)) caminho += $"/{parametro}";
            using (var proxy = new HttpClient())
            {
                var mensagem = new HttpRequestMessage(metodo, caminho);
                if (metodo == HttpMethod.Post && corpo != null)
                {
                    var json = JsonConvert.SerializeObject(corpo);
                    mensagem.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                var resposta = await proxy.SendAsync(mensagem);
                var texto = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(texto);
            }
        }
    }
}
