using System.Xml.Serialization;

namespace BibliotecaCentral.WebService.AutorizarNota
{
    public struct CorpoResponse
    {
        [XmlAttribute]
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string verAplic { get; set; }
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public ushort cUF { get; set; }
        public string dhRecbto { get; set; }
        public Recibo infRec { get; set; }

        public struct Recibo
        {
            public string nRec { get; set; }
            public int tMed { get; set; }
        }
    }
}
