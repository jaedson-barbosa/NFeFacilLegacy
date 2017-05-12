using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Microsoft.EntityFrameworkCore;
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
        private AplicativoContext DB { get; }
        internal ControllerSincronizacaoDadosBase()
        {
            DB = new AplicativoContext();
            DB.ChangeTracker.AutoDetectChangesEnabled = false;
            DB.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [UriFormat("/Dados/{senha}")]
        public IPostResponse ClienteServidorAsync(int senha, [FromContent] DadosBase pacote)
        {
            var item = new ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.DadoBase
            };
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
            finally
            {
                DB.Dispose();
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
            try
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                    new DadosBase
                    {
                        Emitentes = DB.Emitentes.Where(x => x.UltimaData > momento).Include(x => x.endereco).ToList(),
                        Clientes = DB.Clientes.Where(x => x.UltimaData > momento).Include(x => x.endereco).ToList(),
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
            finally
            {
                DB.Dispose();
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
                        Emitentes = DB.Emitentes.Include(x => x.endereco).ToList(),
                        Clientes = DB.Clientes.Include(x => x.endereco).ToList(),
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
            finally
            {
                DB.Dispose();
            }
        }
    }
}
