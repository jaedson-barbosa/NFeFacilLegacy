using BibliotecaCentral.IBGE;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    public static class Consultacao
    {
        public static async Task<Response> ConsultarAsync(bool teste, string chaveNota, Estado UF)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.Consultar);
            return await new GerenciadorGeral<Request, Response>()
                .EnviarAsync(new RequisicaoSOAP<Request>(new Cabecalho(UF.Codigo, "3.10"), new Request
                {
                    consSitNFe = new CorpoRequest(chaveNota)
                }, conjunto));
        }
    }
}
