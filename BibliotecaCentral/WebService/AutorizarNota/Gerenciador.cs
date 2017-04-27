using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    public static class Gerenciador
    {
        public static async Task<Response> AutorizarAsync(int UF, params NFe[] xmls)
        {
            using (var conexao = new Conexao<IAutorizaNFe>(ConjuntoServicos.Autorizar.Endereco))
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
