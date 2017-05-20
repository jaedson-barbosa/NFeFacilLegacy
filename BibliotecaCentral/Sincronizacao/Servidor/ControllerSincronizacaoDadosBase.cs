using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoDadosBase
    {
        [UriFormat("/Dados/{senha}")]
        public IPostResponse ClienteServidorAsync(int senha, [FromContent] DadosBase pacote)
        {
            var item = new ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.DadoBase
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);

                    var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(DB);
                    Mudanca.AdicionarEmitentes(pacote.Emitentes);
                    Mudanca.AdicionarClientes(pacote.Clientes);
                    Mudanca.AdicionarMotoristas(pacote.Motoristas);
                    Mudanca.AdicionarProdutos(pacote.Produtos);
                    var resposta = new PostResponse(PostResponse.ResponseStatus.Created);

                    item.SucessoSolicitacao = true;
                    DB.Add(item);
                    DB.SaveChanges();
                    return resposta;
                }
                catch (Exception e)
                {
                    item.SucessoSolicitacao = false;
                    DB.Add(item);
                    DB.SaveChanges();
                    throw e;
                }
            }
        }

        [UriFormat("/Dados/{senha}/{ultimaSincronizacaoCliente}")]
        public IGetResponse ServidorCliente(int senha, long ultimaSincronizacaoCliente)
        {
            DateTime momento = DateTime.FromBinary(ultimaSincronizacaoCliente);
            if (ultimaSincronizacaoCliente > 10) momento = momento.AddSeconds(-10);
            var item = new ResultadoSincronizacaoServidor()
            {
                TipoDadoSolicitado = (int)TipoDado.DadoBase
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);

                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                        new DadosBase
                        {
                            Emitentes = DB.Emitentes.Where(x => x.UltimaData > momento).ToList(),
                            Clientes = DB.Clientes.Where(x => x.UltimaData > momento).ToList(),
                            Motoristas = DB.Motoristas.Where(x => x.UltimaData > momento).ToList(),
                            Produtos = DB.Produtos.Where(x => x.UltimaData > momento).ToList()
                        });

                    item.SucessoSolicitacao = true;
                    item.MomentoRequisicao = DateTime.Now;
                    DB.Add(item);
                    DB.SaveChanges();
                    return resposta;
                }
                catch (Exception e)
                {
                    item.SucessoSolicitacao = false;
                    item.MomentoRequisicao = DateTime.Now;
                    DB.Add(item);
                    DB.SaveChanges();
                    throw e;
                }
            }
        }

        [UriFormat("/DadosCompleto/{senha}")]
        public IGetResponse SincronizacaoCompleta(int senha, [FromContent] DadosBase pacote)
        {
            var item = new ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.DadoBase
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);

                    var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(DB);
                    Mudanca.AnalisarAdicionarEmitentes(pacote.Emitentes);
                    Mudanca.AnalisarAdicionarClientes(pacote.Clientes);
                    Mudanca.AnalisarAdicionarMotoristas(pacote.Motoristas);
                    Mudanca.AnalisarAdicionarProdutos(pacote.Produtos);

                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                        new DadosBase
                        {
                            Emitentes = DB.Emitentes.ToList(),
                            Clientes = DB.Clientes.ToList(),
                            Motoristas = DB.Motoristas.ToList(),
                            Produtos = DB.Produtos.ToList()
                        });

                    item.SucessoSolicitacao = true;
                    DB.Add(item);
                    DB.SaveChanges();
                    return resposta;
                }
                catch (Exception e)
                {
                    item.SucessoSolicitacao = false;
                    DB.Add(item);
                    DB.SaveChanges();
                    throw e;
                }
            }
        }
    }
}
