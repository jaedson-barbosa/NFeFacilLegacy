using NFeFacil.ModeloXML;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct EnviNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote", Order = 0)]
        public long IdLote { get; set; }

        [XmlElement("indSinc", Order = 1)]
        public int IndicadorSincronismo { get; set; }

        [XmlElement(ElementName = nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 2, IsNullable = false)]
        public NFe[] NFe { get; set; }

        public EnviNFe(params NFe[] xmls)
        {
            Versao = "3.10";
            IdLote = xmls[0].Informacoes.identificacao.Numero;
            IndicadorSincronismo = 0;
            NFe = xmls;
        }
    }

    [XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct EnviNFCe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("idLote", Order = 0)]
        public long IdLote { get; set; }

        [XmlElement("indSinc", Order = 1)]
        public int IndicadorSincronismo { get; set; }

        [XmlElement(ElementName = nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 2, IsNullable = false)]
        public NFCe[] NFe { get; set; }

        public EnviNFCe(params NFCe[] xmls)
        {
            Versao = "3.10";
            IdLote = xmls[0].Informacoes.identificacao.Numero;
            IndicadorSincronismo = 0;
            NFe = xmls;
        }
    }
}
