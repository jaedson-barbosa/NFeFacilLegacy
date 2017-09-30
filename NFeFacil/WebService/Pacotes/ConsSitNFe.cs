using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("consSitNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct ConsSitNFe
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string xServ { get; set; }
        public string chNFe { get; set; }

        public ConsSitNFe(string chave)
        {
            versao = "3.10";
            tpAmb = 1;
            xServ = "CONSULTAR";
            chNFe = chave;
        }
    }
}
