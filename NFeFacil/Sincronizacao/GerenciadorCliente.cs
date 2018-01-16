using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
using NFeFacil.Log;
using NFeFacil.Sincronizacao.Pacotes;
using System.Threading.Tasks;
using System.Text;
using System;
using NFeFacil.Sincronizacao.FastServer;
using System.Xml.Linq;
using Windows.Networking.Sockets;
using Windows.Networking;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace NFeFacil.Sincronizacao
{
    public sealed class GerenciadorCliente
    {
        private Popup Log { get; } = Popup.Current;

        public async Task EstabelecerConexao(int senha)
        {
            var (objeto, mensagem) = await RequestAsync<string>("BrechaSeguranca", senha, null);
            if (objeto != null)
            {
                SenhaPermanente = int.Parse(objeto);
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
                envio.ToXElement<ConjuntoDadosBase>(),
                UltimaSincronizacao.ToBinary().ToString());
            if (objeto != null)
            {
                objeto.AnalisarESalvar(UltimaSincronizacao);
                UltimaSincronizacao = objeto.InstanteSincronizacao;

                var envioNotas = new ConjuntoNotasFiscais(UltimaSincronizacaoNotas);
                var recebNotas = await RequestAsync<ConjuntoNotasFiscais>(
                    $"SincronizarNotasFiscais",
                    SenhaPermanente,
                    envioNotas.ToXElement<ConjuntoNotasFiscais>(),
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
                envio.ToXElement<ConjuntoDadosBase>(),
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
                    envioNotas.ToXElement<ConjuntoNotasFiscais>(),
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

        async Task<(T objeto,string mensagem)> RequestAsync<T>(string nomeMetodo, int senha, XNode corpo, string parametroExtra = null) where T : class
        {
            string caminho = $"/{nomeMetodo}/{senha}";
            if (parametroExtra != null) caminho += $"/{parametroExtra}";
            var envio = new XElement("Envio",
                new XElement("Content", corpo),
                new XElement("Uri", caminho));
            var str = envio.ToString(SaveOptions.DisableFormatting);
            byte[] bytes = Encoding.UTF8.GetBytes($"{str.Length.ToString("0000000000")}{str}");

            using (var socket = new StreamSocket())
            {
                await socket.ConnectAsync(new HostName(IPServidor), "8080");
                using (var output = socket.OutputStream)
                {
                    await output.WriteAsync(bytes.AsBuffer());
                }
                using (var input = socket.InputStream)
                {
                    var buffer = new Windows.Storage.Streams.Buffer(10);
                    var result = await input.ReadAsync(buffer, 10, InputStreamOptions.None);
                    var tamStr = Encoding.UTF8.GetString(result.ToArray());
                    var tamanho = uint.Parse(tamStr);

                    buffer = new Windows.Storage.Streams.Buffer(tamanho);
                    result = await input.ReadAsync(buffer, tamanho, InputStreamOptions.None);
                    var response = result.AsStream().FromStream<RestResponse>();
                    var respCont = response.ContentData;
                    if (response.Sucesso)
                    {
                        var retorno = typeof(T) == typeof(string) ? respCont as T : respCont.FromString<T>();
                        return (retorno, null);
                    }
                    else return (null, respCont);
                }
            }
        }
    }
}
