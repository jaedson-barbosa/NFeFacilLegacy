using BibliotecaCentral.IBGE;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService
{
    internal struct GerenciadorGeral<Envio, Resposta>
    {
        DadosServico Enderecos { get; }
        (int CodigoUF, string VersaoDados) cabecalho;

        internal GerenciadorGeral(Estado uf, DadosServico enderecos)
        {
            Enderecos = enderecos;
            cabecalho = (uf.Codigo, "3.10");
        }

        internal async Task<Resposta> EnviarAsync(Envio corpo)
        {
            var repo = new Repositorio.Certificados();
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            handler.ClientCertificates.Add(await repo.ObterCertificadoEscolhidoAsync());

            var proxy = new HttpClient(handler);
            proxy.DefaultRequestHeaders.Add("SOAPAction", Enderecos.Metodo);

            var resposta = await proxy.PostAsync(Enderecos.Endereco, ObterConteudoRequisicao(corpo));
            var xml = XElement.Load(await resposta.Content.ReadAsStreamAsync());
            return ObterConteudoCorpo(xml).FromXElement<Resposta>();

            XNode ObterConteudoCorpo(XElement soap)
            {
                return soap.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/")).FirstNode;
            }
        }

        HttpContent ObterConteudoRequisicao(Envio corpo)
        {
            string texto = string.Format(
                Extensoes.ObterRecurso("RequisicaoSOAP"),
                Enderecos.Servico,
                cabecalho.CodigoUF,
                cabecalho.VersaoDados,
                corpo.ToXElement<Envio>().ToString(SaveOptions.DisableFormatting));
            return new StringContent(texto, Encoding.UTF8, "text/xml");
        }
    }
}
