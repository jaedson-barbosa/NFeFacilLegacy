using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;
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
            var item = new ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.NotaFiscal
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);

                    await new Repositorio.MudancaOtimizadaBancoDados(DB)
                        .AdicionarNotasFiscais(pacote.DIs.Zip(pacote.XMLs, (di, xml) => new { di, xml })
                        .ToDictionary(x => x.di, x => x.xml));
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

        [UriFormat("/Notas/{senha}/{ultimaSincronizacaoCliente}")]
        public async Task<IGetResponse> ServidorCliente(int senha, long ultimaSincronizacaoCliente)
        {
            DateTime momento = DateTime.FromBinary(ultimaSincronizacaoCliente);
            if (ultimaSincronizacaoCliente > 10) momento = momento.AddSeconds(-10);
            var item = new ResultadoSincronizacaoServidor()
            {
                TipoDadoSolicitado = (int)TipoDado.NotaFiscal
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);

                    var conjunto = from nota in DB.NotasFiscais
                                   where nota.UltimaData > momento
                                   join xml in await new PastaNotasFiscais().Registro() on nota.Id equals xml.nome
                                   select new { DI = nota, XML = xml.xml };
                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                        new NotasFiscais
                        {
                            DIs = conjunto.Select(x => x.DI).ToList(),
                            XMLs = conjunto.Select(x => x.XML).ToList()
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

        [UriFormat("/NotasCompleto/{senha}")]
        public async Task<IGetResponse> SincronizacaoCompleta(int senha, [FromContent] NotasFiscais pacote)
        {
            var item = new ResultadoSincronizacaoServidor()
            {
                MomentoRequisicao = DateTime.Now,
                TipoDadoSolicitado = (int)TipoDado.NotaFiscal
            };
            using (var DB = new AplicativoContext())
            {
                try
                {
                    if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                        throw new SenhaErrada(senha);

                    await new Repositorio.MudancaOtimizadaBancoDados(DB)
                        .AdicionarNotasFiscais(pacote.DIs.Zip(pacote.XMLs, (di, xml) => new { di, xml })
                        .ToDictionary(x => x.di, x => x.xml));

                    var conjunto = from nota in DB.NotasFiscais
                                   join xml in await new PastaNotasFiscais().Registro() on nota.Id equals xml.nome
                                   select new { DI = nota, XML = xml.xml };
                    var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                        new NotasFiscais
                        {
                            DIs = conjunto.Select(x => x.DI).ToList(),
                            XMLs = conjunto.Select(x => x.XML).ToList()
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
