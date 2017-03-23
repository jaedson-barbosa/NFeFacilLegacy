using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NFeFacil.WebService
{
    internal sealed class GerenciadorGeral<Envio, Resposta>
    {
        internal delegate Message EnvioSíncrono(Message envio);
        internal delegate Task<Message> EnvioAssícrono(Message envio);

        private EnvioSíncrono Processar { get; }
        private EnvioAssícrono ProcessarAsync { get; }

        private DadosServico Caminhos { get; }

        internal GerenciadorGeral(EnvioSíncrono sinc, EnvioAssícrono assinc, DadosServico caminhos)
        {
            Processar = sinc;
            ProcessarAsync = assinc;
            Caminhos = caminhos;
        }

        internal Resposta Enviar(Envio envio, int UF)
        {
            var xml = envio.ToXElement<Envio>(Caminhos.Servico);
            var resultado = Processar(
                ProcessarMensagem(xml.CreateReader(),
                Caminhos.Servico,
                Caminhos.Metodo,
                UF));
            var stringXml = resultado.GetReaderAtBodyContents().ReadOuterXml();
            var xmlResultado = XElement.Parse(stringXml);
            return xmlResultado.FromXElement<Resposta>();
        }

        internal async Task<Resposta> EnviarAsync(Envio envio, int UF)
        {
            var xml = envio.ToXElement<Envio>(Caminhos.Servico);
            var resultado = await ProcessarAsync(
                ProcessarMensagem(xml.CreateReader(),
                Caminhos.Servico,
                Caminhos.Metodo,
                UF));
            var stringXml = await resultado.GetReaderAtBodyContents().ReadOuterXmlAsync();
            var xmlResultado = XElement.Parse(stringXml);
            return xmlResultado.FromXElement<Resposta>();
        }

        private static Message ProcessarMensagem(XmlReader corpo, string servico, string metodo, int UF)
        {
            var envio = Message.CreateMessage(MessageVersion.Soap11, metodo, corpo);
            var header = new MessageHeader<Cabeçalho>(new Cabeçalho(UF, "3.10"));
            envio.Headers.Add(header.GetUntypedHeader("nfeCabecMsg", servico));
            return envio;
        }

        private struct Cabeçalho : IXmlSerializable
        {
            public int cUF { get; set; }
            public string versaoDados { get; set; }

            public Cabeçalho(int cUF, string versao)
            {
                this.cUF = cUF;
                versaoDados = versao;
            }

            public XmlSchema GetSchema() => null;
            public void ReadXml(XmlReader reader) { }
            public void WriteXml(XmlWriter writer)
            {
                writer.WriteElementString(nameof(versaoDados), versaoDados);
                writer.WriteElementString(nameof(cUF), cUF.ToString());
            }
        }
    }
}
