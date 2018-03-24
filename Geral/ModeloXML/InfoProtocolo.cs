using System.Xml.Serialization;

namespace NFeFacil.ModeloXML
{
    public sealed class InfoProtocolo
    {
        [XmlAttribute]
        public string Id { get; set; }
        public int tpAmb { get; set; }
        public string verAplic { get; set; }
        public string chNFe { get; set; }
        public string dhRecbto { get; set; }
        public ulong nProt { get; set; }
        public string digVal { get; set; }
        public int cStat { get; set; }
        public string xMotivo { get; set; }
    }
}