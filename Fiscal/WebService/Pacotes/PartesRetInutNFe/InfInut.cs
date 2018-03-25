using System.Xml.Serialization;

namespace Fiscal.WebService.Pacotes.PartesRetInutNFe
{
    public struct InfInut
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public int TipoAmbiente { get; set; }

        [XmlElement("verAplic", Order = 1)]
        public string VersaoAplicativo { get; set; }

        [XmlElement("cStat", Order = 2)]
        public int StatusResposta { get; set; }

        [XmlElement("xMotivo", Order = 3)]
        public string DescricaoResposta { get; set; }

        [XmlElement("cUF", Order = 4)]
        public ushort Estado { get; set; }

        [XmlElement("ano", Order = 5)]
        public int Ano { get; set; }

        [XmlElement(Order = 6)]
        public string CNPJ { get; set; }

        [XmlElement("mod", Order = 7)]
        public int ModeloDocumento { get; set; }

        [XmlElement("serie", Order = 8)]
        public int SerieNFe { get; set; }

        [XmlElement("nNFIni", Order = 9)]
        public int InicioNumeracao { get; set; }

        [XmlElement("nNFFin", Order = 10)]
        public int FinalNumeracao { get; set; }

        [XmlElement("dhRecbto", Order = 11)]
        public string DataHoraProcessamento { get; set; }

        [XmlElement("nProt", Order = 12)]
        public long NumeroProtocolo { get; set; }
    }
}
