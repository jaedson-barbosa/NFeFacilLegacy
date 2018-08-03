using Fiscal.Certificacao.LAN.Pacotes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BaseGeral.IBGE;
using Fiscal.Certificacao.LAN;
using Fiscal.Certificacao;
using BaseGeral;
using BaseGeral.Certificacao;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Fiscal.WebService
{
    public sealed class GerenciadorGeral<Envio, Resposta>
    {
        public DadosServico Enderecos { get; }
        int CodigoUF { get; }
        string VersaoDados { get; }

        public event ProgressChangedEventHandler ProgressChanged;
        async Task OnProgressChanged(int conc)
        {
            if (ProgressChanged != null) await ProgressChanged(this, conc);
        }

        public readonly string[] Etapas = new string[4]
        {
            "Preparar conexão",
            "Obter conteúdo da requisição",
            "Enviar requisição",
            "Processar resposta"
        };

        public GerenciadorGeral(Estado uf, Operacoes operacao, bool teste, bool isNFCe)
        {
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao, isNFCe);
            CodigoUF = uf.Codigo;
            VersaoDados = operacao == Operacoes.RecepcaoEvento ? "1.00" : "4.00";
        }

        public GerenciadorGeral(string siglaOuNome, Operacoes operacao, bool teste, bool isNFCe)
            : this(Estados.Buscar(siglaOuNome), operacao, teste, isNFCe)
        { }

        public GerenciadorGeral(ushort codigo, Operacoes operacao, bool teste, bool isNFCe)
            : this(Estados.Buscar(codigo), operacao, teste, isNFCe)
        { }

        public async Task<Resposta> EnviarAsync(Envio corpo, bool addNamespace = false)
        {
            var origem = ConfiguracoesCertificacao.Origem;
            
            if (origem == OrigemCertificado.Importado)
            {
                await OnProgressChanged(1);
                var bind = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                var client = new Autorizacao.NFeAutorizacao4SoapClient(bind, new EndpointAddress(Enderecos.Endereco));
                var loja = new X509Store();
                loja.Open(OpenFlags.ReadOnly);
                client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser,
                    StoreName.My,
                    X509FindType.FindBySerialNumber,
                    loja.Certificates[0].SerialNumber);
                await OnProgressChanged(2);

                await client.OpenAsync();
                var resp = await client.nfeAutorizacaoLoteAsync(ObterXmlConteudoRequisicao(corpo, addNamespace));
                await OnProgressChanged(3);

                var retorno = resp.nfeResultMsg.FromXElement<Resposta>();
                await OnProgressChanged(4);
                return retorno;
            }
            else
            {
                var envio = new RequisicaoEnvioDTO()
                {
                    Cabecalho = new CabecalhoRequisicao()
                    {
                        Nome = "SOAPAction",
                        Valor = Enderecos.Metodo
                    },
                    Conteudo = XElement.Parse(ObterConteudoRequisicao(corpo, addNamespace)),
                    Uri = Enderecos.Endereco,
                    TipoConteudo = ObterTipoConteudo()
                };

                using (var cliente = new HttpClient())
                {
                    var uri = new Uri($"http://{OperacoesServidor.RootUri}:{(origem == OrigemCertificado.Cliente ? 2020 : 1010)}/EnviarRequisicao");
                    await OnProgressChanged(1);

                    var xml = envio.ToXElement<RequisicaoEnvioDTO>().ToString(SaveOptions.DisableFormatting);
                    var conteudo = new StringContent(xml, Encoding.UTF8, "text/xml");
                    await OnProgressChanged(2);

                    var resposta = await cliente.PostAsync(uri, conteudo);
                    await OnProgressChanged(3);

                    var xmlResposta = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                    var retorno = xmlResposta.FromXElement<Resposta>();
                    await OnProgressChanged(4);

                    return retorno;
                }
            }
        }

        string ObterConteudoRequisicao(Envio corpo, bool addNamespace)
        {
            return ObterXmlConteudoRequisicao(corpo, addNamespace).ToString(SaveOptions.DisableFormatting);
        }

        XElement ObterXmlConteudoRequisicao(Envio corpo, bool addNamespace)
        {
            var xml = corpo.ToXElement<Envio>();
            if (addNamespace)
            {
                const string namespaceNFe = "http://www.portalfiscal.inf.br/nfe";
                xml.Element(XName.Get("NFe", namespaceNFe)).SetAttributeValue("xmlns", namespaceNFe);
            }

            var servico = Enderecos.Servico;
            string namespaceXML = DefinicoesPermanentes.UsarSOAP12
                ? "http://www.w3.org/2003/05/soap-envelope"
                : "http://schemas.xmlsoap.org/soap/envelope/";
            var teste = new XElement(XName.Get("Envelope", namespaceXML),
                new XElement(XName.Get("Body", namespaceXML),
                    new XElement(Name("nfeDadosMsg"), xml)));
            return teste;

            XName Name(string original) => XName.Get(original, servico);
        }

        string ObterTipoConteudo()
        {
            const string Soap11 = "text/xml";
            const string Soap12 = "application/soap+xml";
            return DefinicoesPermanentes.UsarSOAP12 ? Soap12 : Soap11;
        }
    }
}
