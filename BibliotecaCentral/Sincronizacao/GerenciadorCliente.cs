using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace BibliotecaCentral.Sincronizacao
{
    public sealed class GerenciadorCliente
    {
        private ILog Log { get; }
        public ItensBD.ResultadoSincronizacaoCliente Resultado { get; private set; }

        public GerenciadorCliente(ILog log)
        {
            Log = log;
        }

        public async Task EstabelecerConexao(int senha)
        {
            var info = await EnviarAsync<InfoSegurancaConexao>("BrechaSeguranca", Método.GET, senha, null);
            SenhaPermanente = info.Senha;
            Log.Escrever(TitulosComuns.Sucesso, "Chave de segurança decodificada e salva com sucesso.");
        }

        public async Task Sincronizar(DadosSincronizaveis sincronizar, bool isBackground)
        {
            try
            {
                ItensSincronizados quantNotas = new ItensSincronizados(), quantDados = new ItensSincronizados();

                var config = await ObterConfiguracoes();
                if (config.Notas && config.DadosBase && sincronizar == DadosSincronizaveis.Tudo)
                {
                    quantNotas = await SincronizarNotas();
                    quantDados = await SincronizarDadosBase();
                    Log.Escrever(TitulosComuns.Sucesso, "Foram sincronizados tanto notas fiscais quanto dados base para criação das notas fiscais.");
                }
                else if (config.Notas && sincronizar == DadosSincronizaveis.Tudo || sincronizar == DadosSincronizaveis.NotasFiscais)
                {
                    quantNotas = await SincronizarNotas();
                    Log.Escrever(TitulosComuns.Sucesso, "Apenas as notas fiscais puderam ser sincronizadas porque o servidor bloqueou a sincronização de dados base.");
                }
                else if (config.DadosBase && sincronizar == DadosSincronizaveis.Tudo || sincronizar == DadosSincronizaveis.DadosBase)
                {
                    quantDados = await SincronizarDadosBase();
                    Log.Escrever(TitulosComuns.Sucesso, "Apenas os dados base puderam ser sincronizados porque o servidor bloqueou a sincronização de dados base.");
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, "Nada pôde ser sincronizado porque o servidor bloqueou a sincronização do tipo de dado solicitado(s).");
                }

                using (var db = new AplicativoContext())
                {
                    db.Add(new ItensBD.ResultadoSincronizacaoCliente
                    {
                        PodeSincronizarDadoBase = config.DadosBase,
                        PodeSincronizarNota = config.Notas,
                        NumeroDadosEnviados = quantDados.Enviados,
                        NumeroDadosRecebidos = quantDados.Recebidos,
                        NumeroNotasEnviadas = quantNotas.Enviados,
                        NumeroNotasRecebidas = quantNotas.Recebidos,
                        MomentoSincronizacao = config.HoraRequisição,
                        SincronizacaoAutomatica = isBackground
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, $"Erro: {e.Message}");
            }

            async Task<ConfiguracoesServidor> ObterConfiguracoes()
            {
                return await EnviarAsync<Pacotes.ConfiguracoesServidor>($"Configuracoes", Método.GET, SenhaPermanente, null);
            }

            async Task<ItensSincronizados> SincronizarDadosBase()
            {
                var envio = ProcessamentoDadosBase.Obter();
                await EnviarAsync<string>($"Dados", Método.POST, SenhaPermanente, envio);
                var receb = await EnviarAsync<DadosBase>($"Dados", Método.GET, SenhaPermanente, null);
                ProcessamentoDadosBase.Salvar(receb);
                return new ItensSincronizados(CalcularTotal(envio), CalcularTotal(receb));

                int CalcularTotal(DadosBase dados)
                {
                    return dados.Clientes.Count() + dados.Emitentes.Count() + dados.Motoristas.Count() + dados.Produtos.Count();
                }
            }

            async Task<ItensSincronizados> SincronizarNotas()
            {
                var envio = await ProcessamentoNotas.ObterAsync();
                await EnviarAsync<string>("Notas", Método.POST, SenhaPermanente, envio);
                var receb = await EnviarAsync<NotasFiscais>("Notas", Método.GET, SenhaPermanente, null).ConfigureAwait(false);
                await ProcessamentoNotas.SalvarAsync(receb);
                return new ItensSincronizados(envio.XMLs.Count(), receb.XMLs.Count());
            }
        }

        async Task<T> EnviarAsync<T>(string nomeMétodo, Método metodoConexao, int senha, PacoteBase corpo) where T : class
        {
            // Define os parâmetros básicos da requisição
            string caminho = $"http://{IPServidor}:8080/{nomeMétodo}/{metodoConexao.ToString()}/{senha}";
            var webRequest = System.Net.WebRequest.CreateHttp(caminho);
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

        enum Método
        {
            GET,
            POST
        }
    }

    public enum DadosSincronizaveis
    {
        Tudo,
        DadosBase,
        NotasFiscais
    }
}
