using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using Windows.ApplicationModel.Core;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerInformacoes
    {
        private AplicativoContext DB { get; }
        internal ControllerInformacoes()
        {
            DB = new AplicativoContext();
            DB.ChangeTracker.AutoDetectChangesEnabled = false;
            DB.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [UriFormat("/BrechaSeguranca/{senha}")]
        public IGetResponse BrechaSeguranca(int senha)
        {
            var item = new ItensBD.ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.SenhaDeAcesso
            };
            try
            {
                if ((bool)CoreApplication.Properties["BrechaAberta"])
                {
                    if (senha != SenhaTemporária)
                        throw new SenhaErrada(senha);

                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK, new InfoSegurancaConexao
                    {
                        Senha = ConfiguracoesSincronizacao.SenhaPermanente
                    });

                    item.SucessoSolicitacao = true;
                    DB.Add(item);
                    DB.SaveChanges();
                    return resposta;
                }
                else
                {
                    item.SucessoSolicitacao = true;
                    DB.Add(item);
                    DB.SaveChanges();
                    return new GetResponse(GetResponse.ResponseStatus.NotFound);
                }
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

        [UriFormat("/Configuracoes/{senha}")]
        public IGetResponse Configuracoes(int senha)
        {
            var item = new ItensBD.ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.Configuracao
            };
            try
            {
                if (senha != SenhaPermanente)
                    throw new SenhaErrada(senha);

                var resposta = new GetResponse(GetResponse.ResponseStatus.OK, new Pacotes.ConfiguracoesServidor
                {
                    DadosBase = SincDadoBase,
                    Notas = SincNotaFiscal
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
