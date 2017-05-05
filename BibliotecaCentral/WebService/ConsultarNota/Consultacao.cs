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

        public async Task<Response> ConsultarAsync(bool teste, string chaveNota)
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
