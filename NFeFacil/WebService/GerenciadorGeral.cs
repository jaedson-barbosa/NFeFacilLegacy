using NFeFacil.Certificacao;
using NFeFacil.Certificacao.LAN;
using Comum.Pacotes;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NFeFacil.IBGE;

namespace NFeFacil.WebService
{
    public struct GerenciadorGeral<Envio, Resposta>
    {
        public DadosServico Enderecos { get; }
        int CodigoUF { get; }
        string VersaoDados{get;}

        public GerenciadorGeral(Estado uf, Operacoes operacao, bool teste)
        {
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            CodigoUF = uf.Codigo;
            VersaoDados = operacao == Operacoes.RecepcaoEvento ? Enderecos.VersaoRecepcaoEvento : "3.10";
        }

        public GerenciadorGeral(string siglaOuNome, Operacoes operacao, bool teste)
        {
            var uf = Estados.Buscar(siglaOuNome);
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            CodigoUF = uf.Codigo;
            VersaoDados = operacao == Operacoes.RecepcaoEvento ? Enderecos.VersaoRecepcaoEvento : "3.10";
        }

        public GerenciadorGeral(ushort codigo, Operacoes operacao, bool teste)
        {
            var uf = Estados.Buscar(codigo);
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            CodigoUF = uf.Codigo;
            VersaoDados = operacao == Operacoes.RecepcaoEvento ? Enderecos.VersaoRecepcaoEvento : "3.10";
        }

        public async Task<Resposta> EnviarAsync(Envio corpo)
        {
            if (ConfiguracoesCertificacao.Origem == OrigemCertificado.Importado)
            {
                using (var proxy = new HttpClient(new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Automatic,
                    UseDefaultCredentials = true
                }, true))
                {
                    proxy.DefaultRequestHeaders.Add("SOAPAction", Enderecos.Metodo);
                    var conteudo = new StringContent(ObterConteudoRequisicao(corpo), Encoding.UTF8, "text/xml");
                    var resposta = await proxy.PostAsync(Enderecos.Endereco, conteudo);
                    var xml = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                    return ObterConteudoCorpo(xml).FromXElement<Resposta>();
                }
            }
            else
            {
                var op = new OperacoesServidor();
                return await op.EnviarRequisicaoIntermediada<Resposta>(new RequisicaoEnvioDTO()
                {
                    Cabecalho = new CabecalhoRequisicao()
                    {
                        Nome = "SOAPAction",
                        Valor = Enderecos.Metodo
                    },
                    Conteudo = XElement.Parse(ObterConteudoRequisicao(corpo)),
                    Uri = Enderecos.Endereco
                });
            }

            XNode ObterConteudoCorpo(XElement soap)
            {
                var nome = XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/");
                var item = soap.Element(nome);
                if (item == null)
                {
                    nome = XName.Get("Body", "http://www.w3.org/2003/05/soap-envelope");
                    item = soap.Element(nome);
                }
                var casca = (XElement)item.FirstNode;
                return casca.FirstNode;
            }
        }

        string ObterConteudoRequisicao(Envio corpo)
        {
            return string.Format(ExtensoesPrincipal.ObterRecurso("RequisicaoSOAP"),
                Enderecos.Servico, CodigoUF, VersaoDados,
                corpo.ToXElement<Envio>().ToString(SaveOptions.DisableFormatting));
        }
    }
}
