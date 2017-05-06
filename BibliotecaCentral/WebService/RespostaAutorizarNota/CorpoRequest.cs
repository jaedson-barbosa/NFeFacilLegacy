using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    [XmlRoot("consReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public struct CorpoRequest
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string nRec { get; set; }

        public CorpoRequest(int tipoAmbiente, string numeroRecibo)
        {
            versao = "3.10";
            tpAmb = tipoAmbiente;
            nRec = numeroRecibo;
        }
    }
}
