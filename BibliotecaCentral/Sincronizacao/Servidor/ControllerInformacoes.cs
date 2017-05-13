using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using Windows.ApplicationModel.Core;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerInformacoes
    {
        [UriFormat("/BrechaSeguranca/{senha}")]
        public IGetResponse BrechaSeguranca(int senha)
        {
            var item = new ItensBD.ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.SenhaDeAcesso
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if ((bool)CoreApplication.Properties["BrechaAberta"])
                    {
                        if (senha != SenhaTemporária)
                            throw new SenhaErrada(senha);

                        var resposta = new GetResponse(GetResponse.ResponseStatus.OK, new InfoSegurancaConexao
                        {
                            Senha = SenhaPermanente
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
            using (var DB = new AplicativoContext())
            {
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
            }
        }
    }
}
