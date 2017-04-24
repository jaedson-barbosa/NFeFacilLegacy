using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    /*    <tpAmb>1</tpAmb>
    <verAplic>SVRS201601160932</verAplic>
    <nRec>253002481313326</nRec>
    <cStat>517</cStat>
    <xMotivo>Rejeicao: Falha no schema XML – inexiste atributo versao na tag raiz da mensagem</xMotivo>
    <cUF>25</cUF>
    <dhRecbto>2017-03-12T15:28:46-03:00</dhRecbto>
*/
    public struct CorpoResponse
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
