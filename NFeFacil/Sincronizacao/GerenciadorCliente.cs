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
            var envio = new ConjuntoDadosBase(UltimaSincronizacao);
            var receb = await RequestAsync<ConjuntoDadosBase>(
                $"SincronizarDadosBase",
                SenhaPermanente,
                envio,
                UltimaSincronizacao.ToBinary().ToString());
            receb.AnalisarESalvar();
            UltimaSincronizacao = receb.InstanteSincronizacao;

            var envioNotas = new ConjuntoNotasFiscais();
            envioNotas.AtualizarPadrao();
            var recebNotas = await RequestAsync<ConjuntoNotasFiscais>(
                $"SincronizarNotasFiscais",
                SenhaPermanente,
                envio,
                UltimaSincronizacaoNotas.ToBinary().ToString());
            receb.AnalisarESalvar();
            UltimaSincronizacaoNotas = recebNotas.InstanteSincronizacao;

            Log.Escrever(TitulosComuns.Sucesso, "Sincronização simples concluida.");
        }

        internal async Task SincronizarTudo()
        {
            var envio = new ConjuntoDadosBase();
            envio.AtualizarPadrao();
            var receb = await RequestAsync<ConjuntoDadosBase>(
                $"SincronizarDadosBase",
                SenhaPermanente,
                envio,
                DateTime.MinValue.ToBinary().ToString());
            receb.AnalisarESalvar();
            UltimaSincronizacao = receb.InstanteSincronizacao;

            var envioNotas = new ConjuntoNotasFiscais();
            envioNotas.AtualizarPadrao();
            var recebNotas = await RequestAsync<ConjuntoNotasFiscais>(
                $"SincronizarNotasFiscais",
                SenhaPermanente,
                envioNotas,
                DateTime.MinValue.ToBinary().ToString());
            receb.AnalisarESalvar();
            UltimaSincronizacaoNotas = recebNotas.InstanteSincronizacao;

            Log.Escrever(TitulosComuns.Sucesso, "Sincronização total concluida.");
        }

        async Task<T> RequestAsync<T>(string nomeMetodo, int senha, object corpo, string parametroExtra = null) where T : class
        {
            string caminho = $"http://{IPServidor}:8080/{nomeMetodo}/{senha}";
            if (parametroExtra != null) caminho += $"/{parametroExtra}";
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
                if (resposta.IsSuccessStatusCode)
                {
                    var objeto = JsonConvert.DeserializeObject<T>(texto);
                    return objeto;
                }
                else
                {
                    var objeto = JsonConvert.DeserializeXmlNode(texto);
                    
                    return null;
                }
            }
        }
    }
}
