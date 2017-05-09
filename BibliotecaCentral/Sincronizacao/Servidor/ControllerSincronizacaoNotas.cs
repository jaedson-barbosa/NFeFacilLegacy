using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Threading.Tasks;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoNotas
    {
        [UriFormat("/Notas/{senha}")]
        public async Task<IPostResponse> ClienteServidor(int senha, [FromContent] NotasFiscais pacote)
        {
            using (var db = new AplicativoContext())
            {
                var item = new ItensBD.ResultadoSincronizacaoServidor()
                {
                    MomentoRequisicao = pacote.HoraRequisição,
                    TipoDadoSolicitado = (int)TipoDado.NotaFiscal
                };
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);
                    await new ProcessamentoNotas(db).SalvarAsync(pacote);
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

        [UriFormat("/Notas/{senha}")]
        public async Task<IGetResponse> ServidorCliente(int senha)
        {
            using (var db = new AplicativoContext())
            {
                var item = new ItensBD.ResultadoSincronizacaoServidor()
                {
                    MomentoRequisicao = DateTime.Now,
                    TipoDadoSolicitado = (int)TipoDado.NotaFiscal
                };
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);
                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK, await new ProcessamentoNotas(db).ObterAsync());

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
    }
}
