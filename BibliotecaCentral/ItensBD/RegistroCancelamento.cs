using BibliotecaCentral.WebService.Pacotes;
using System.Xml.Serialization;

namespace BibliotecaCentral.ItensBD
{
    public sealed class RegistroCancelamento
    {
        public string ChaveNFe { get; set; }
        public int TipoAmbiente { get; set; }
        public string DataHoraEvento { get; set; }
        public string XML { get; set; }
    }

    [XmlRoot("procEventoNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct ProcEventoCancelamento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("evento")]
        public Evento[] Eventos { get; set; }

        [XmlElement("retEvento")]
        public ResultadoEvento[] RetEvento { get;set;}
    }
}
