using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.WebService
{
    internal struct GerenciadorGeral<Envio, Resposta>
    {
        internal delegate Task<Message> EnvioAssícrono(Message envio);

        private DadosServico Caminhos { get; }

        internal GerenciadorGeral(DadosServico caminhos)
        {
            Caminhos = caminhos;
        }

        internal async Task<Resposta> EnviarAsync(Envio envio, int UF, EnvioAssícrono ProcessarAsync)
        {
            var xml = envio.ToXElement<Envio>(Caminhos.Servico);
            var resultado = await ProcessarAsync(
                ProcessarMensagem(xml,
                Caminhos.Servico,
                Caminhos.Metodo,
                UF));
            var stringXml = await resultado.GetReaderAtBodyContents().ReadOuterXmlAsync();
            var xmlResultado = XElement.Parse(stringXml);
            return xmlResultado.FromXElement<Resposta>();
        }

        private static Message ProcessarMensagem(object corpo, string servico, string metodo, int UF)
        {
            var envio = Message.CreateMessage(MessageVersion.Soap11, metodo, corpo);
            var header = new MessageHeader<Cabecalho>(new Cabecalho(UF, "3.10"));
            envio.Headers.Add(header.GetUntypedHeader("nfeCabecMsg", servico));
            return envio;
        }
    }

    internal struct RequisicaoSOAP<TipoCorpo>
    {
        Cabecalho cabecalho;
        TipoCorpo corpo;
        DadosServico conjunto;

        internal RequisicaoSOAP(Cabecalho cabec, TipoCorpo corpo, DadosServico dados)
        {
            cabecalho = cabec;
            this.corpo = corpo;
            conjunto = dados;
        }

        internal HttpContent ObterConteudoRequisicao()
        {
            string texto = string.Format(
                Extensoes.ObterRecurso("RequisicaoSOAP"),
                conjunto.Servico,
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
