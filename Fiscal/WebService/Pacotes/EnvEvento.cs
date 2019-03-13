using Fiscal.Certificacao;
using Fiscal.WebService.Pacotes.PartesEnvEvento;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes
{
    [XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class EnvEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public int IdLote { get; set; }

        [XmlElement("evento")]
        public Evento[] Eventos { get; set; }

        public EnvEvento() { }
        public EnvEvento(params InformacoesEvento[] eventos)
        {
            Versao = "1.00";
            IdLote = 0;
            Eventos = new Evento[eventos.Length];
            for (int i = 0; i < eventos.Length; i++)
            {
                Eventos[i] = new Evento("1.00", eventos[i]);
            }
        }

        public async Task<(bool, string)> PrepararEventos(AssinaFacil assinador, X509Certificate2 cert)
        {
            for (int i = 0; i < Eventos.Length; i++)
            {
                var evento = Eventos[i];
                assinador.Nota = evento;
                var resposta = await assinador.Assinar<Evento>(cert, evento.InfEvento.Id, "infEvento");
                if (!resposta.Item1)
                {
                    return (false, resposta.Item2);
                }
            }
            return (true, "Documento assinado com sucesso.");
        }
    }
}
