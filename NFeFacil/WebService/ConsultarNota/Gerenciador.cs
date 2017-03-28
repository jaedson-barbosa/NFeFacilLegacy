using System.Threading.Tasks;

namespace NFeFacil.WebService.ConsultarNota
{
    public static class Gerenciador
    {
        public static async Task<Response> ConsultarAsync(string chaveNota, int UF)
        {
            using (Conexao<IConsultaNFe> Conexao = new Conexao<IConsultaNFe>(ConjuntoServicos.Consultar.Endereco))
            {
                var consulta = Conexao.EstabelecerConexão();
                return await new GerenciadorGeral<Request, Response>(
                    consulta.Consultar, consulta.ConsultarAsync,
                    ConjuntoServicos.Consultar).EnviarAsync(new Request
                    {
                        consSitNFe = new CorpoRequest(chaveNota)
                    }, UF);
            }
        }
    }
}
