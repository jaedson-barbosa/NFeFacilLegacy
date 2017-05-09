using BibliotecaCentral.IBGE;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService
{
    public struct GerenciadorGeral<Envio, Resposta>
    {
        DadosServico enderecos;
        (int CodigoUF, string VersaoDados) cabecalho;

        public GerenciadorGeral(Estado uf, Operacoes operacao, bool teste)
        {
            enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            cabecalho = (uf.Codigo, "3.10");
        }

        public GerenciadorGeral(string siglaOuNome, Operacoes operacao, bool teste)
        {
            var uf = Estados.Buscar(siglaOuNome);
            enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            cabecalho = (uf.Codigo, "3.10");
        }

        public GerenciadorGeral(ushort codigo, Operacoes operacao, bool teste)
        {
            var uf = Estados.Buscar(codigo);
            enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            cabecalho = (uf.Codigo, "3.10");
        }

        public async Task<Resposta> EnviarAsync(Envio corpo)
        {
            var repo = new Certificacao.Certificados();
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            handler.ClientCertificates.Add(await repo.ObterCertificadoEscolhidoAsync());

            using (var proxy = new HttpClient(handler, true))
            {
                proxy.DefaultRequestHeaders.Add("SOAPAction", enderecos.Metodo);

                var resposta = await proxy.PostAsync(enderecos.Endereco, ObterConteudoRequisicao(corpo));
                var xml = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                return ObterConteudoCorpo(xml).FromXElement<Resposta>();
            }

            XElement ObterConteudoCorpo(XElement soap)
            {
                var casca = soap.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/")).FirstNode as XElement;
                return casca.FirstNode;
            }
        }

        HttpContent ObterConteudoRequisicao(Envio corpo)
        {
            string texto = string.Format(
                Extensoes.ObterRecurso("RequisicaoSOAP"),
                enderecos.Servico,
                cabecalho.CodigoUF,
                cabecalho.VersaoDados,
                corpo.ToXElement<Envio>().ToString(SaveOptions.DisableFormatting));
            return new StringContent(texto, Encoding.UTF8, "text/xml");
        }
    }
}
