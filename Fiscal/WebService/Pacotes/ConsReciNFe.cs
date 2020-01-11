using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes
{
    [XmlRoot("consReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct ConsReciNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public int TipoAmbiente { get; set; }

        [XmlElement("nRec", Order = 1)]
        public string NumeroRecibo { get; set; }

        public ConsReciNFe(int tipoAmbiente, string numeroRecibo)
        {
            Versao = "4.00";
            TipoAmbiente = tipoAmbiente;
            NumeroRecibo = numeroRecibo;
        }
    }
}
