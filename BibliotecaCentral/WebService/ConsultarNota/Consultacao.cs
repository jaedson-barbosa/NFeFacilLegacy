using BibliotecaCentral.IBGE;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    public static class Consultacao
    {
        public static async Task<Response> ConsultarAsync(bool teste, string chaveNota, Estado UF)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.Consultar);
            using (Conexao<IConsultaNFe> Conexao = new Conexao<IConsultaNFe>(conjunto.Endereco))
            {
                return await new GerenciadorGeral<Request, Response>(conjunto)
                    .EnviarAsync(new Request
                    {
                        consSitNFe = new CorpoRequest(chaveNota)
                    }, UF.Codigo, Conexao.EstabelecerConexão().ConsultarAsync);
            }
        }
    }
}
