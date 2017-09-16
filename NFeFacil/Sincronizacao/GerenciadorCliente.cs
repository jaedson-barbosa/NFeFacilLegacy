﻿using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
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
            ItensSincronizados quantNotas = new ItensSincronizados(), quantDados = new ItensSincronizados();

            using (var db = new AplicativoContext())
            {
                quantNotas = await SincronizarNotas(db);
                quantDados = await SincronizarDadosBase(db);
                Log.Escrever(TitulosComuns.Sucesso, "Foram sincronizados tanto notas fiscais quanto dados base para criação das notas fiscais.");
                db.SaveChanges();
            }

            async Task<ItensSincronizados> SincronizarDadosBase(AplicativoContext contexto)
            {
                var momento = UltimaSincronizacao;
                var receb = await EnviarAsync<ConjuntoBanco>($"Dados", HttpMethod.Get, SenhaPermanente, null, momento.ToBinary().ToString());

                var envio = new ConjuntoBanco
                {
                    Emitentes = contexto.Emitentes.Where(x => x.UltimaData > momento).ToList(),
                    Clientes = contexto.Clientes.Where(x => x.UltimaData > momento).ToList(),
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

                int CalcularTotal(ConjuntoBanco dados)
                {
                    return dados.Clientes.Count + dados.Emitentes.Count + dados.Motoristas.Count + dados.Produtos.Count;
                }
            }

            async Task<ItensSincronizados> SincronizarNotas(AplicativoContext contexto)
            {
                var momento = UltimaSincronizacao;
                var receb = await EnviarAsync<NotasFiscais>("Notas", HttpMethod.Get, SenhaPermanente, null, momento.ToBinary().ToString());

                var conjunto = from item in contexto.NotasFiscais
                               where item.UltimaData > momento
                               select item;
                var envio = new NotasFiscais
                {
                    DIs = conjunto.ToList(),
                };

                await EnviarAsync<string>("Notas", HttpMethod.Post, SenhaPermanente, envio);
                new Repositorio.MudancaOtimizadaBancoDados(contexto)
                    .AdicionarNotasFiscais(receb.DIs);

                return new ItensSincronizados(envio.DIs.Count, receb.DIs.Count);
            }
        }

        public async Task SincronizarTudo()
        {
            ItensSincronizados quantNotas = new ItensSincronizados(), quantDados = new ItensSincronizados();

            using (var db = new AplicativoContext())
            {
                quantNotas = await SincronizarNotas(db);
                quantDados = await SincronizarDadosBase(db);
                Log.Escrever(TitulosComuns.Sucesso, "Foram sincronizados tanto notas fiscais quanto dados base para criação das notas fiscais.");
                db.SaveChanges();
            }

            async Task<ItensSincronizados> SincronizarDadosBase(AplicativoContext contexto)
            {
                var envio = new ConjuntoBanco
                {
                    Emitentes = contexto.Emitentes.ToList(),
                    Clientes = contexto.Clientes.ToList(),
                    Motoristas = contexto.Motoristas.ToList(),
                    Produtos = contexto.Produtos.ToList()
                }; ;
                var receb = await EnviarAsync<ConjuntoBanco>($"DadosCompleto", HttpMethod.Get, SenhaPermanente, envio);

                var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(contexto);
                Mudanca.AnalisarAdicionarEmitentes(receb.Emitentes);
                Mudanca.AnalisarAdicionarClientes(receb.Clientes);
                Mudanca.AnalisarAdicionarMotoristas(receb.Motoristas); ;
                Mudanca.AnalisarAdicionarProdutos(receb.Produtos);

                return new ItensSincronizados(CalcularTotal(envio), CalcularTotal(receb));

                int CalcularTotal(ConjuntoBanco dados)
                {
                    return dados.Clientes.Count + dados.Emitentes.Count + dados.Motoristas.Count + dados.Produtos.Count;
                }
            }

            async Task<ItensSincronizados> SincronizarNotas(AplicativoContext contexto)
            {
                var envio = new NotasFiscais
                {
                    DIs = contexto.NotasFiscais.ToList(),
                };
                var receb = await EnviarAsync<NotasFiscais>("NotasCompleto", HttpMethod.Get, SenhaPermanente, envio);

                new Repositorio.MudancaOtimizadaBancoDados(contexto)
                    .AdicionarNotasFiscais(receb.DIs);

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
}
