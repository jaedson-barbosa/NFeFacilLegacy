﻿using BibliotecaCentral.IBGE;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService
{
    public struct GerenciadorGeral<Envio, Resposta>
    {
        public DadosServico Enderecos { get; }
        (int CodigoUF, string VersaoDados) cabecalho;

        public GerenciadorGeral(Estado uf, Operacoes operacao, bool teste)
        {
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            cabecalho = (uf.Codigo, "1.00");
        }

        public GerenciadorGeral(string siglaOuNome, Operacoes operacao, bool teste)
        {
            var uf = Estados.Buscar(siglaOuNome);
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            cabecalho = (uf.Codigo, "3.10");
        }

        public GerenciadorGeral(ushort codigo, Operacoes operacao, bool teste)
        {
            var uf = Estados.Buscar(codigo);
            Enderecos = new EnderecosConexao(uf.Sigla).ObterConjuntoConexao(teste, operacao);
            cabecalho = (uf.Codigo, "3.10");
        }

        public async Task<Resposta> EnviarAsync(Envio corpo)
        {
            using (var proxy = new HttpClient(new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                UseDefaultCredentials = true
            }, true))
            {
                proxy.DefaultRequestHeaders.Add("SOAPAction", Enderecos.Metodo);
                var resposta = await proxy.PostAsync(Enderecos.Endereco, ObterConteudoRequisicao(corpo));
                var xml = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                return ObterConteudoCorpo(xml).FromXElement<Resposta>();
            }

            XNode ObterConteudoCorpo(XElement soap)
            {
                var casca = soap.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/")).FirstNode as XElement;
                return casca.FirstNode;
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
