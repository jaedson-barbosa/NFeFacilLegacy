using BibliotecaCentral.IBGE;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    public struct Autorizacao
    {
        Estado UF { get; }

        public Autorizacao(Estado estado)
        {
            UF = estado;
        }

        public Autorizacao(string siglaOuNome)
        {
            UF = Estados.Buscar(siglaOuNome);
        }

        public Autorizacao(ushort codigo)
        {
            UF = Estados.Buscar(codigo);
        }

        public async Task<Response> AutorizarAsync(bool teste, params NFe[] xmls)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.Autorizar);
            return await new GerenciadorGeral<Request, Response>(UF, conjunto)
                .EnviarAsync(new Request
                {
                    enviNFe = new CorpoRequest(xmls, xmls[0].Informações.identificação.Numero)
                });
        }
    }
}
