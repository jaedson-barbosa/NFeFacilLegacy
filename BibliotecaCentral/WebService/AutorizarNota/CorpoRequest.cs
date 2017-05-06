using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    [XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct CorpoRequest
    {
        [XmlAttribute]
        public string versao { get; set; }
        public long idLote { get; set; }
        public int indSinc { get; set; }
        [XmlElement(ElementName = nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
        public NFe[] NFe { get; set; }

        public CorpoRequest(NFe[] xmls, long numeroPrimeiraNota)
        {
            versao = "3.10";
            idLote = numeroPrimeiraNota;
            indSinc = 0;
            NFe = xmls;
        }
    }
}
