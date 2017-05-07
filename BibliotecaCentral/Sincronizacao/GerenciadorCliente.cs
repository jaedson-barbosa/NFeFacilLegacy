using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;

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
            var info = await EnviarAsync<InfoSegurancaConexao>("BrechaSeguranca", HttpMethod.Get, senha, null);
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
                return await EnviarAsync<ConfiguracoesServidor>($"Configuracoes", HttpMethod.Get, SenhaPermanente, null);
            }

            async Task<ItensSincronizados> SincronizarDadosBase()
            {
                var envio = ProcessamentoDadosBase.Obter();
                await EnviarAsync<string>($"Dados", HttpMethod.Post, SenhaPermanente, envio);
                var receb = await EnviarAsync<DadosBase>($"Dados", HttpMethod.Get, SenhaPermanente, null);
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
                await EnviarAsync<string>("Notas", HttpMethod.Post, SenhaPermanente, envio);
                var receb = await EnviarAsync<NotasFiscais>("Notas", HttpMethod.Get, SenhaPermanente, null).ConfigureAwait(false);
                await ProcessamentoNotas.SalvarAsync(receb);
                return new ItensSincronizados(envio.XMLs.Count(), receb.XMLs.Count());
            }
        }

        async Task<T> EnviarAsync<T>(string nomeMetodo, HttpMethod metodo, int senha, PacoteBase corpo)
        {
            string caminho = $"http://{IPServidor}:8080/{nomeMetodo}/{senha}";
            using (var proxy = new HttpClient())
            {
                var mensagem = new HttpRequestMessage(metodo, caminho);
                if (metodo == HttpMethod.Post && corpo != null)
                {
                    var json = JsonConvert.SerializeObject(corpo);
                    mensagem.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                var resposta = await proxy.SendAsync(mensagem);
                return JsonConvert.DeserializeObject<T>(await resposta.Content.ReadAsStringAsync());
            }
        }
    }

    public enum DadosSincronizaveis
    {
        Tudo,
        DadosBase,
        NotasFiscais
    }
}
