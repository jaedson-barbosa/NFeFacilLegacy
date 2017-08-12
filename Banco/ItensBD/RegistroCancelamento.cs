using Banco.WebService.Pacotes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Banco.ItensBD
{
    public sealed class RegistroCancelamento
    {
        [Key]
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
