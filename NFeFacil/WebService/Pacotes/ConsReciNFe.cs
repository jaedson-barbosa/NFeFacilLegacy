using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes
{
    [XmlRoot("consReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct ConsReciNFe
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string nRec { get; set; }

        public ConsReciNFe(int tipoAmbiente, string numeroRecibo)
        {
            versao = "3.10";
            tpAmb = tipoAmbiente;
            nRec = numeroRecibo;
        }
    }
}
