using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Threading.Tasks;
using BibliotecaCentral.ItensBD;

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
                var item = new ResultadoSincronizacaoServidor()
                {
                    MomentoRequisicao = DateTime.Now,
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

        [UriFormat("/Notas/{senha}/{ultimaSincronizacaoCliente}")]
        public async Task<IGetResponse> ServidorCliente(int senha, long ultimaSincronizacaoCliente)
        {
            using (var db = new AplicativoContext())
            {
                DateTime momento = DateTime.FromBinary(ultimaSincronizacaoCliente);
                if (ultimaSincronizacaoCliente > 10) momento = momento.AddSeconds(-10);
                var item = new ResultadoSincronizacaoServidor()
                {
                    TipoDadoSolicitado = (int)TipoDado.NotaFiscal
                };
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);
                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                        await new ProcessamentoNotas(db).ObterAsync(momento));

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
