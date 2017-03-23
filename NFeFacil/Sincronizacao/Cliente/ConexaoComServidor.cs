using static NFeFacil.Configuracoes.ConfiguracoesSincronizacao;
using Newtonsoft.Json;
using NFeFacil.Sincronizacao.Pacotes;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NFeFacil.Sincronizacao.Cliente
{
    public abstract class ConexaoComServidor
    {
        protected async Task<T> SendRequest<T>(string nomeMétodo, Método metodoConexao, int senha, PacoteBase corpo) where T : class
        {
            // Define os parâmetros básicos da requisição
            string caminho = $"http://{IPServidor}:8080/{nomeMétodo}/{metodoConexao.ToString()}/{senha}";
            var webRequest = WebRequest.CreateHttp(caminho);
            webRequest.Accept = "application/json";
            webRequest.Method = metodoConexao.ToString();

            // Caso a requisição deva ter um corpo ele deve ser enviado
            if (metodoConexao == Método.POST && corpo != null)
            {
                webRequest.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(corpo);
                var requestStream = await webRequest.GetRequestStreamAsync();
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    await streamWriter.WriteAsync(json);
                }
            }

            // Caso tudo dê certo, a resposta vem aqui
            using (var response = await webRequest.GetResponseAsync())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    if (typeof(T) == typeof(string)) return await streamReader.ReadToEndAsync() as T;
                    else return JsonConvert.DeserializeObject<T>(await streamReader.ReadToEndAsync());
                }
            }
        }

        protected enum Método
        {
            GET,
            POST
        }
    }
}
