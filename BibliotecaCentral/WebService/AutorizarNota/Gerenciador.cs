using BibliotecaCentral.IBGE;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    public static class Gerenciador
    {
        public static async Task<Response> AutorizarAsync(bool teste, Estado UF, params NFe[] xmls)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.Autorizar);
            using (var conexao = new Conexao<IAutorizaNFe>(conjunto.Endereco))
            {
                return await new GerenciadorGeral<Request, Response>(conjunto)
                    .EnviarAsync(new Request
                    {
                        enviNFe = new CorpoRequest(xmls, xmls[0].Informações.identificação.Numero)
                    }, UF.Codigo, conexao.EstabelecerConexão().nfeAutorizacaoLoteAsync);
            }
        }
    }
}
