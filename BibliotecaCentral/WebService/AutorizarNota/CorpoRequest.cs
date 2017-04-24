using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    public struct CorpoRequest
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int idLote { get; set; }
        public int indSinc { get; set; }
        [XmlElement(ElementName = nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public NFe[] NFe { get; set; }

        public CorpoRequest(NFe[] xmls, int numeroPrimeiraNota)
        {
            versao = "3.10";
            idLote = numeroPrimeiraNota;
            indSinc = 0;
            NFe = xmls;
        }
    }
}
