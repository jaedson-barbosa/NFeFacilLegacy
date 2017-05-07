using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.Pacotes
{
    [XmlRoot("retConsReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct RetConsReciNFe
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string verAplic { get; set; }
        public string nRec { get; set; }
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public int cUF { get; set; }
        public string dhRecbto { get; set; }
        public ProtocoloNFe protNFe { get; set; }
    }
}
