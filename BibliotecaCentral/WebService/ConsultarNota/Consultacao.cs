using BibliotecaCentral.IBGE;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    public struct Consultacao
    {
        Estado UF { get; }

        public Consultacao(Estado estado)
        {
            UF = estado;
        }

        public Consultacao(string siglaOuNome)
        {
            UF = Estados.Buscar(siglaOuNome);
        }

        public Consultacao(ushort codigo)
        {
            UF = Estados.Buscar(codigo);
        }

        public async Task<CorpoResponse> ConsultarAsync(bool teste, string chaveNota)
        {
            return await new GerenciadorGeral<CorpoRequest, CorpoResponse>(UF, Operacoes.Consultar, teste)
                .EnviarAsync(new CorpoRequest(chaveNota));
        }
    }
}
