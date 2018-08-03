using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes
{
    [XmlRoot("consSitNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct ConsSitNFe
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public int TipoAmbiente { get; set; }

        [XmlElement("xServ", Order = 1)]
        public string DescricaoServico { get; set; }

        [XmlElement("chNFe", Order = 2)]
        public string ChaveNFe { get; set; }

        public ConsSitNFe(string chave, bool teste)
        {
            Versao = "4.00";
            TipoAmbiente = teste ? 2 : 1;
            DescricaoServico = "CONSULTAR";
            ChaveNFe = chave;
        }
    }
}
