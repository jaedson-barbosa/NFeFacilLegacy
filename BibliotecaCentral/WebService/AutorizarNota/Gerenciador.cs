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
                var autoriza = conexao.EstabelecerConexão();
                return await new GerenciadorGeral<Request, Response>(
                    autoriza.nfeAutorizacaoLote, autoriza.nfeAutorizacaoLoteAsync,
                    conjunto).EnviarAsync(new Request
                    {
                        enviNFe = new CorpoRequest(xmls, (int)xmls[0].Informações.identificação.Numero)
                    }, UF.Codigo);
            }
        }
    }
}
