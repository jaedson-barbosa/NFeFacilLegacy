using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso
{
    public sealed class InfoProtocolo
    {
        [XmlAttribute]
        public string Id;
        public int tpAmb;
        public string verAplic;
        public string chNFe;
        public string dhRecbto;
        public ulong nProt;
        public string digVal;
        public int cStat;
        public string xMotivo;
    }
}