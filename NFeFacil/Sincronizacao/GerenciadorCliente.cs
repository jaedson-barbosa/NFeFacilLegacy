using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
using NFeFacil.Log;
using NFeFacil.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System;
using NFeFacil.PacotesBanco;

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
            var (objeto, mensagem) = await RequestAsync<InfoSegurancaConexao>("BrechaSeguranca", senha, null);
            if (objeto != null)
            {
                SenhaPermanente = objeto.Senha;
                Log.Escrever(TitulosComuns.Sucesso, "Chave de segurança decodificada e salva com sucesso.");
            }
            else
            {
                Log.Escrever(TitulosComuns.Erro, mensagem);
            }
        }

        internal async Task Sincronizar()
        {
            string mensagemErro = null;

            var envio = new ConjuntoDadosBase(UltimaSincronizacao);
            var (objeto, mensagem) = await RequestAsync<ConjuntoDadosBase>(
                $"SincronizarDadosBase",
                SenhaPermanente,
                envio,
                UltimaSincronizacao.ToBinary().ToString());
            if (objeto != null)
            {
                objeto.AnalisarESalvar(UltimaSincronizacao);
                UltimaSincronizacao = objeto.InstanteSincronizacao;

                var envioNotas = new ConjuntoNotasFiscais(UltimaSincronizacaoNotas);
                var recebNotas = await RequestAsync<ConjuntoNotasFiscais>(
                    $"SincronizarNotasFiscais",
                    SenhaPermanente,
                    envioNotas,
                    UltimaSincronizacaoNotas.ToBinary().ToString());
                if (recebNotas.objeto != null)
                {
                    recebNotas.objeto.AnalisarESalvar();
                    UltimaSincronizacaoNotas = recebNotas.objeto.InstanteSincronizacao;
                }
                else
                {
                    mensagemErro = recebNotas.mensagem;
                }
            }
            else
            {
                mensagemErro = mensagem;
            }

            if (string.IsNullOrEmpty(mensagemErro))
            {
                Log.Escrever(TitulosComuns.Sucesso, "Sincronização simples concluida.");
            }
            else
            {
                Log.Escrever(TitulosComuns.Erro, mensagemErro);
            }
        }

        internal async Task SincronizarTudo()
        {
            string mensagemErro = null;

            var envio = new ConjuntoDadosBase();
            envio.AtualizarPadrao();
            var (objeto, mensagem) = await RequestAsync<ConjuntoDadosBase>(
                $"SincronizarDadosBase",
                SenhaPermanente,
                envio,
                DateTime.MinValue.ToBinary().ToString());
            if (objeto != null)
            {
                objeto.AnalisarESalvar(DateTime.MinValue);
                UltimaSincronizacao = objeto.InstanteSincronizacao;

                var envioNotas = new ConjuntoNotasFiscais();
                envioNotas.AtualizarPadrao();
                var recebNotas = await RequestAsync<ConjuntoNotasFiscais>(
                    $"SincronizarNotasFiscais",
                    SenhaPermanente,
                    envioNotas,
                    DateTime.MinValue.ToBinary().ToString());
                if (recebNotas.objeto != null)
                {
                    recebNotas.objeto.AnalisarESalvar();
                    UltimaSincronizacaoNotas = recebNotas.objeto.InstanteSincronizacao;
                }
                else
                {
                    mensagemErro = recebNotas.mensagem;
                }
            }
            else
            {
                mensagemErro = mensagem;
            }

            if (string.IsNullOrEmpty(mensagemErro))
            {
                Log.Escrever(TitulosComuns.Sucesso, "Sincronização completa concluida.");
            }
            else
            {
                Log.Escrever(TitulosComuns.Erro, mensagemErro);
            }
        }

        async Task<(T objeto,string mensagem)> RequestAsync<T>(string nomeMetodo, int senha, object corpo, string parametroExtra = null) where T : class
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
                    return (objeto, null);
                }
                else
                {
                    var objeto = JsonConvert.DeserializeXmlNode(texto, "XML");
                    var partes = objeto.ChildNodes.Item(0).ChildNodes;
                    string detalhesErro = null;
                    for (int i = 0; i < partes.Count; i++)
                    {
                        if (partes[i].Name == "Message")
                        {
                            detalhesErro = partes[i].InnerText;
                        }
                    }
                    return (null, $"Ocorreu um erro durante a execução da requisição de identificação \"{nomeMetodo}\".\r\n" +
                        $"Esta é a mensagem do servidor: {detalhesErro}");
                }
            }
        }
    }
}
