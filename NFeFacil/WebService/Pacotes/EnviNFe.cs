using NFeFacil.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct EnviNFe
    {
        [XmlAttribute]
        public string versao { get; set; }
        public long idLote { get; set; }
        public int indSinc { get; set; }
        [XmlElement(ElementName = nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
        public NFe[] NFe { get; set; }

        public EnviNFe(long numeroPrimeiraNota, params NFe[] xmls)
        {
            versao = "3.10";
            idLote = numeroPrimeiraNota;
            indSinc = 0;
            NFe = xmls;
        }
    }
}
