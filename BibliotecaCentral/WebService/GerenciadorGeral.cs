using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService
{
    internal struct GerenciadorGeral<Envio, Resposta>
    {
        internal async Task<Resposta> EnviarAsync(RequisicaoSOAP<Envio> requisicao)
        {
            var repo = new Repositorio.Certificados();
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            handler.ClientCertificates.Add(await repo.ObterCertificadoEscolhidoAsync());
            var proxy = new HttpClient(handler);
            proxy.DefaultRequestHeaders.Add("SOAPAction", requisicao.Enderecos.Metodo);
            var resposta = await proxy.PostAsync(requisicao.Enderecos.Endereco, requisicao.ObterConteudoRequisicao());
            return ObterConteudoCorpo(XElement.Load(await resposta.Content.ReadAsStreamAsync())).FromXElement<Resposta>();

            XNode ObterConteudoCorpo(XElement soap)
            {
                return soap.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/")).FirstNode;
            }
        }
    }

    internal struct RequisicaoSOAP<TipoCorpo>
    {
        Cabecalho cabecalho;
        TipoCorpo corpo;
        internal DadosServico Enderecos { get; }

        internal RequisicaoSOAP(Cabecalho cabec, TipoCorpo corpo, DadosServico dados)
        {
            cabecalho = cabec;
            this.corpo = corpo;
            Enderecos = dados;
        }

        internal HttpContent ObterConteudoRequisicao()
        {
            string texto = string.Format(
                Extensoes.ObterRecurso("RequisicaoSOAP"),
                Enderecos.Servico,
                cabecalho.CodigoUF,
                cabecalho.VersaoDados,
                corpo.ToXElement<TipoCorpo>().ToString(SaveOptions.DisableFormatting));
            return new StringContent(texto, Encoding.UTF8, "text/xml");
        }
    }

    public struct Cabecalho
    {
        public int CodigoUF { get; set; }
        public string VersaoDados { get; set; }

        public Cabecalho(int cUF, string versao)
        {
            this.CodigoUF = cUF;
            VersaoDados = versao;
        }
    }
}
