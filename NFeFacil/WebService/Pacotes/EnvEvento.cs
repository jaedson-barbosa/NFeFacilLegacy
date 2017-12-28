using NFeFacil.WebService.Pacotes.PartesEnvEvento;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct EnvEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote")]
        public int IdLote { get; set; }

        [XmlElement("evento")]
        public Evento[] Eventos { get; set; }

        public EnvEvento(string versao, params InformacoesEvento[] eventos)
        {
            Versao = versao;
            IdLote = 0;
            Eventos = new Evento[eventos.Length];
            for (int i = 0; i < eventos.Length; i++)
            {
                Eventos[i] = new Evento(versao, eventos[i]);
            }
        }

        public async Task PrepararEventos()
        {
            for (int i = 0; i < Eventos.Length; i++)
            {
                await Eventos[i].Preparar();
            }
        }
    }
}
