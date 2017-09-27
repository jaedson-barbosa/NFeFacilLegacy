using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
using NFeFacil.Log;
using NFeFacil.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System;

namespace NFeFacil.Sincronizacao
{
    public sealed class GerenciadorCliente
    {
        private ILog Log { get; }

        public GerenciadorCliente()
        {
            Log = Popup.Current;
        }

        public async Task EstabelecerConexao(int senha)
        {
            var info = await RequestAsync<InfoSegurancaConexao>("BrechaSeguranca", senha, null);
            SenhaPermanente = info.Senha;
            Log.Escrever(TitulosComuns.Sucesso, "Chave de segurança decodificada e salva com sucesso.");
        }

        internal async Task Sincronizar()
        {
            using (var db = new AplicativoContext())
            {
                var momento = UltimaSincronizacao;

                var receb = await RequestAsync<ConjuntoBanco>(
                    $"Sincronizar",
                    SenhaPermanente,
                    new ConjuntoBanco(db, momento));
                receb.AnalisarESalvar(db);

                Log.Escrever(TitulosComuns.Sucesso, "Sincronização simples concluida.");
                db.SaveChanges();

                UltimaSincronizacao = DateTime.Now;
            }
        }

        internal async Task SincronizarTudo()
        {
            using (var db = new AplicativoContext())
            {
                var receb = await RequestAsync<ConjuntoBanco>(
                    $"Sincronizar",
                    SenhaPermanente,
                    new ConjuntoBanco(db));
                receb.AnalisarESalvar(db);

                Log.Escrever(TitulosComuns.Sucesso, "Sincronização total concluida.");
                db.SaveChanges();

                UltimaSincronizacao = DateTime.Now;
            }
        }

        async Task<T> RequestAsync<T>(string nomeMetodo, int senha, object corpo)
        {
            string caminho = $"http://{IPServidor}:8080/{nomeMetodo}/{senha}";
            using (var proxy = new HttpClient())
            {
                var mensagem = new HttpRequestMessage(HttpMethod.Get, caminho);
                if (corpo != null)
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
