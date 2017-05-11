using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using BibliotecaCentral.ItensBD;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCentral.Sincronizacao
{
    public sealed class GerenciadorCliente
    {
        private ILog Log { get; }
        public ResultadoSincronizacaoCliente Resultado { get; private set; }

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

                var config = await EnviarAsync<ConfiguracoesServidor>($"Configuracoes", HttpMethod.Get, SenhaPermanente, null);
                using (var db = new AplicativoContext())
                {
                    if (config.Notas && config.DadosBase && sincronizar == DadosSincronizaveis.Tudo)
                    {
                        quantNotas = await SincronizarNotas(db);
                        quantDados = await SincronizarDadosBase(db);
                        Log.Escrever(TitulosComuns.Sucesso, "Foram sincronizados tanto notas fiscais quanto dados base para criação das notas fiscais.");
                    }
                    else if (config.Notas && sincronizar == DadosSincronizaveis.Tudo || sincronizar == DadosSincronizaveis.NotasFiscais)
                    {
                        quantNotas = await SincronizarNotas(db);
                        Log.Escrever(TitulosComuns.Sucesso, "Apenas as notas fiscais puderam ser sincronizadas porque o servidor bloqueou a sincronização de dados base.");
                    }
                    else if (config.DadosBase && sincronizar == DadosSincronizaveis.Tudo || sincronizar == DadosSincronizaveis.DadosBase)
                    {
                        quantDados = await SincronizarDadosBase(db);
                        Log.Escrever(TitulosComuns.Sucesso, "Apenas os dados base puderam ser sincronizados porque o servidor bloqueou a sincronização de notas fiscais.");
                    }
                    else
                    {
                        Log.Escrever(TitulosComuns.ErroSimples, "Nada pôde ser sincronizado porque o servidor bloqueou a sincronização do tipo de dado solicitado(s).");
                    }

                    db.Add(new ResultadoSincronizacaoCliente
                    {
                        PodeSincronizarDadoBase = config.DadosBase,
                        PodeSincronizarNota = config.Notas,
                        NumeroDadosEnviados = quantDados.Enviados,
                        NumeroDadosRecebidos = quantDados.Recebidos,
                        NumeroNotasEnviadas = quantNotas.Enviados,
                        NumeroNotasRecebidas = quantNotas.Recebidos,
                        MomentoSincronizacao = DateTime.Now,
                        SincronizacaoAutomatica = isBackground
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, $"Erro: {e.Message}");
            }

            async Task<ItensSincronizados> SincronizarDadosBase(AplicativoContext contexto)
            {
                var momento = contexto.ResultadosCliente.Count(x => x.PodeSincronizarDadoBase) > 0 ? contexto.ResultadosCliente.Last(x => x.PodeSincronizarDadoBase).MomentoSincronizacao : DateTime.MinValue;
                var receb = await EnviarAsync<DadosBase>($"Dados", HttpMethod.Get, SenhaPermanente, null, momento.ToBinary().ToString());
                var envio = new DadosBase
                {
                    Emitentes = contexto.Emitentes.Where(x => x.UltimaData > momento).Include(x => x.endereco).ToList(),
                    Clientes = contexto.Clientes.Where(x => x.UltimaData > momento).Include(x => x.endereco).ToList(),
                    Motoristas = contexto.Motoristas.Where(x => x.UltimaData > momento).ToList(),
                    Produtos = contexto.Produtos.Where(x => x.UltimaData > momento).ToList()
                }; ;
                await EnviarAsync<string>($"Dados", HttpMethod.Post, SenhaPermanente, envio);
                var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(contexto);
                Mudanca.AdicionarEmitentes(receb.Emitentes);
                Mudanca.AdicionarClientes(receb.Clientes);
                Mudanca.AdicionarMotoristas(receb.Motoristas); ;
                Mudanca.AdicionarProdutos(receb.Produtos);
                return new ItensSincronizados(CalcularTotal(envio), CalcularTotal(receb));

                int CalcularTotal(DadosBase dados)
                {
                    return dados.Clientes.Count + dados.Emitentes.Count + dados.Motoristas.Count + dados.Produtos.Count;
                }
            }

            async Task<ItensSincronizados> SincronizarNotas(AplicativoContext contexto)
            {
                var momento = contexto.ResultadosCliente.Count(x => x.PodeSincronizarNota) > 0 ? contexto.ResultadosCliente.Last(x => x.PodeSincronizarNota).MomentoSincronizacao : DateTime.MinValue;
                var receb = await EnviarAsync<NotasFiscais>("Notas", HttpMethod.Get, SenhaPermanente, null, momento.ToBinary().ToString());

                var conjunto = from item in contexto.NotasFiscais
                               where item.UltimaData > momento
                               join xml in await new PastaNotasFiscais().Registro() on item.Id equals xml.nome
                               select new { DI = item, XML = xml.xml };
                var envio = new NotasFiscais
                {
                    DIs = conjunto.Select(x => x.DI).ToList(),
                    XMLs = conjunto.Select(x => x.XML).ToList()
                };

                await EnviarAsync<string>("Notas", HttpMethod.Post, SenhaPermanente, envio);
                await new Repositorio.MudancaOtimizadaBancoDados(contexto)
                    .AdicionarNotasFiscais(receb.DIs.Zip(receb.XMLs, (di, xml) => new { di, xml })
                    .ToDictionary(x => x.di, x => x.xml));

                return new ItensSincronizados(envio.DIs.Count, receb.DIs.Count);
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

    public enum DadosSincronizaveis
    {
        Tudo,
        DadosBase,
        NotasFiscais
    }
}
