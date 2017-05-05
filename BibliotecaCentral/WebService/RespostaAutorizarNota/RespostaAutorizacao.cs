using BibliotecaCentral.IBGE;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    public struct RespostaAutorizacao
    {
        AutorizarNota.CorpoResponse Recibo { get; }
        Estado UF => Estados.Buscar(Recibo.cUF);

        public RespostaAutorizacao(AutorizarNota.CorpoResponse recibo)
        {
            Recibo = recibo;
        }

        public async Task<Response> ObterRespostaAutorizacao(bool teste)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.RespostaAutorizar);
            return await new GerenciadorGeral<Request, Response>(UF, conjunto)
                .EnviarAsync(new Request
                {
                    consReciNFe = new CorpoRequest(Recibo.tpAmb, Recibo.infRec.nRec)
                });
        }
    }
}
