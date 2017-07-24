using NFeFacil.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;

namespace NFeFacil.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoNotas
    {
        [UriFormat("/Notas/{senha}")]
        public IPostResponse ClienteServidor(int senha, [FromContent] NotasFiscais pacote)
        {
            using (var DB = new AplicativoContext())
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                new Repositorio.MudancaOtimizadaBancoDados(DB)
                    .AdicionarNotasFiscais(pacote.DIs);
                var resposta = new PostResponse(PostResponse.ResponseStatus.Created);

                DB.SaveChanges();
                return resposta;
            }
        }

        [UriFormat("/Notas/{senha}/{ultimaSincronizacaoCliente}")]
        public IGetResponse ServidorCliente(int senha, long ultimaSincronizacaoCliente)
        {
            DateTime momento = DateTime.FromBinary(ultimaSincronizacaoCliente);
            if (ultimaSincronizacaoCliente > 10) momento = momento.AddSeconds(-10);
            using (var DB = new AplicativoContext())
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                var conjunto = from nota in DB.NotasFiscais
                               where nota.UltimaData > momento
                               select nota;
                var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                    new NotasFiscais
                    {
                        DIs = conjunto.ToList(),
                    });

                DB.SaveChanges();
                return resposta;
            }
        }

        [UriFormat("/NotasCompleto/{senha}")]
        public IGetResponse SincronizacaoCompleta(int senha, [FromContent] NotasFiscais pacote)
        {
            using (var DB = new AplicativoContext())
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                new Repositorio.MudancaOtimizadaBancoDados(DB)
                    .AdicionarNotasFiscais(pacote.DIs);

                var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                    new NotasFiscais
                    {
                        DIs = DB.NotasFiscais.ToList(),
                    });

                DB.SaveChanges();
                return resposta;
            }
        }
    }
}
