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
            string texto =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                    "<soap:Header>" +
                        $"<nfeCabecMsg xmlns=\"{conjunto.Servico}\">" +
                            "<cUF>25</cUF>" +
                            "<versaoDados>3.10</versaoDados>" +
                        "</nfeCabecMsg>" +
                    "</soap:Header>" +
                    "<soap:Body>" +
                        new Request
                        {
                            consSitNFe = new CorpoRequest(chaveNota)
                        }.ToXElement<Request>().ToString(SaveOptions.DisableFormatting) +
                    "</soap:Body>" +
                "</soap:Envelope>";
            var resposta = await proxy.PostAsync(conjunto.Endereco, new StringContent(texto, Encoding.UTF8, "text/xml"));
            return ObterConteudoCorpo(XElement.Load(await resposta.Content.ReadAsStreamAsync())).FromXElement<Response>();
        }

        private static XNode ObterConteudoCorpo(XElement soap) => soap.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/")).FirstNode;
    }
}
