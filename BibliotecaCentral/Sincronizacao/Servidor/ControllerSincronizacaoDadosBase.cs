using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoDadosBase
    {
        [UriFormat("/Dados/{senha}")]
        public IPostResponse ClienteServidorAsync(int senha, [FromContent] DadosBase pacote)
        {
            using (var db = new AplicativoContext())
            {
                var item = new ResultadoSincronizacaoServidor()
                {
                    MomentoRequisicao = pacote.HoraRequisição,
                    TipoDadoSolicitado = (int)TipoDado.DadoBase
                };
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);
                    new ProcessamentoDadosBase(db).Salvar(pacote);
                    var resposta = new PostResponse(PostResponse.ResponseStatus.Created);

                    item.SucessoSolicitacao = true;
                    db.Add(item);
                    db.SaveChanges();
                    return resposta;
                }
                catch (Exception e)
                {
                    item.SucessoSolicitacao = false;
                    db.Add(item);
                    db.SaveChanges();
                    throw e;
                }
            }
        }

        [UriFormat("/Dados/{senha}/{ultimaSincronizacaoCliente}")]
        public IGetResponse ServidorCliente(int senha, long ultimaSincronizacaoCliente)
        {
            using (var db = new AplicativoContext())
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
                        new ProcessamentoDadosBase(db).Obter(momento));

                    item.SucessoSolicitacao = true;
                    item.MomentoRequisicao = DateTime.Now;
                    db.Add(item);
                    db.SaveChanges();
                    return resposta;
                }
                catch (Exception e)
                {
                    item.SucessoSolicitacao = false;
                    item.MomentoRequisicao = DateTime.Now;
                    db.Add(item);
                    db.SaveChanges();
                    throw e;
                }
            }
        }
    }
}
