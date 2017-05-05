using BibliotecaCentral.IBGE;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    public static class Consultacao
    {
        public static async Task<Response> ConsultarAsync(bool teste, string chaveNota, Estado UF)
        {
            var conjunto = new EnderecosConexao(UF.Sigla).ObterConjuntoConexao(teste, Operacoes.Consultar);
            /*using (Conexao<IConsultaNFe> Conexao = new Conexao<IConsultaNFe>(conjunto.Endereco))
            {
                return await new GerenciadorGeral<Request, Response>(conjunto)
                    .EnviarAsync(new Request
                    {
                        consSitNFe = new CorpoRequest(chaveNota)
                    }, UF.Codigo, (await Conexao.EstabelecerConexãoAsync()).ConsultarAsync);
            }*/
            var repo = new Repositorio.Certificados();
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            handler.ClientCertificates.Add(await repo.ObterCertificadoEscolhidoAsync());
            var proxy = new HttpClient(handler);
            proxy.DefaultRequestHeaders.Add("SOAPAction", conjunto.Metodo);
            string texto = string.Format(Extensoes.ObterRecurso("RequisicaoSOAP"),
                conjunto.Servico,
                25, "3.10", new Request
                {
                    consSitNFe = new CorpoRequest(chaveNota)
                }.ToXElement<Request>().ToString(SaveOptions.DisableFormatting));
            var resposta = await proxy.PostAsync(conjunto.Endereco, new StringContent(Extensoes.ObterRecurso("RequisicaoSOAP"), Encoding.UTF8, "text/xml"));
            var textoRetorno = await resposta.Content.ReadAsStringAsync();
            return ObterConteudoCorpo(XElement.Load(await resposta.Content.ReadAsStreamAsync())).FromXElement<Response>();
        }

        private static XNode ObterConteudoCorpo(XElement soap) => soap.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/")).FirstNode;
    }
}
