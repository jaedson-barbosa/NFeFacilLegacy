using NFeFacil.ModeloXML.PartesProcesso;
using System.Threading.Tasks;

namespace NFeFacil.WebService.AutorizarNota
{
    public static class Gerenciador
    {
        public static async Task<Response> AutorizarAsync(NFe[] xmls, int UF)
        {
            using (var conexao = new Conexao<IAutorizaNFe>(ConjuntoServicos.Autorizar.endereco))
            {
                var autoriza = conexao.EstabelecerConexão();
                return await new GerenciadorGeral<Request, Response>(
                    autoriza.nfeAutorizacaoLote, autoriza.nfeAutorizacaoLoteAsync,
                    ConjuntoServicos.Autorizar).EnviarAsync(new Request
                    {
                        enviNFe = new CorpoRequest(xmls, (int)xmls[0].Informações.identificação.Numero)
                    }, UF);
            }
        }
    }
}
