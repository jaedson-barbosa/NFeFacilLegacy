using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.ConsultarNota
{
    public struct CorpoRequest
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string xServ { get; set; }
        public string chNFe { get; set; }

        public CorpoRequest(string chave)
        {
            versao = "3.10";
            tpAmb = 1;
            xServ = "CONSULTAR";
            chNFe = chave;
        }
    }
}
