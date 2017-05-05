using BibliotecaCentral.IBGE;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    public static class Autorizacao
    {
        public static async Task<Response> AutorizarAsync(bool teste, Estado UF, params NFe[] xmls)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.Autorizar);
            return await new GerenciadorGeral<Request, Response>()
                .EnviarAsync(new RequisicaoSOAP<Request>(new Cabecalho(UF.Codigo, "3.10"), new Request
                {
                    enviNFe = new CorpoRequest(xmls, xmls[0].Informações.identificação.Numero)
                }, conjunto));
        }
    }
}
