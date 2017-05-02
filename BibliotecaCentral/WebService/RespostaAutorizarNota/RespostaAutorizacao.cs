using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    public static class RespostaAutorizacao
    {
        public static async Task<Response> ObterRespostaAutorizacao(bool teste, AutorizarNota.CorpoResponse recibo)
        {
            var estado = IBGE.Estados.EstadosCache.First(x => x.Codigo == recibo.cUF);
            var conjunto = new EnderecosConexao(estado.Sigla).ObterConjuntoConexao(teste, Operacoes.RespostaAutorizar);
            using (var conexao = new Conexao<IRespostaAutorizaNFe>(conjunto.Endereco))
            {
                return await new GerenciadorGeral<Request, Response>(conjunto)
                    .EnviarAsync(new Request
                    {
                        consReciNFe = new CorpoRequest(recibo.tpAmb, recibo.infRec.nRec)
                    }, recibo.cUF, conexao.EstabelecerConexão().nfeRetAutorizacaoLoteAsync);
            }
        }
    }
}
