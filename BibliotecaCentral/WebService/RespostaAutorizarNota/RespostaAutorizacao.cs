using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    public static class RespostaAutorizacao
    {
        public static async Task<Response> ObterRespostaAutorizacao(bool teste, AutorizarNota.CorpoResponse recibo)
        {
            var estado = IBGE.Estados.Buscar(recibo.cUF);
            var conjunto = new EnderecosConexao(estado.Sigla).ObterConjuntoConexao(teste, Operacoes.RespostaAutorizar);
            return await new GerenciadorGeral<Request, Response>()
                .EnviarAsync(new RequisicaoSOAP<Request>(new Cabecalho(recibo.cUF, "3.10"), new Request
                {
                    consReciNFe = new CorpoRequest(recibo.tpAmb, recibo.infRec.nRec)
                }, conjunto));
        }
    }
}
