using System.Xml;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes.PartesEnvEvento
{
    public struct DetalhamentoEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("descEvento", Order = 0)]
        public string DescricaoEvento { get; set; }

        [XmlElement("nProt", Order = 1)]
        public ulong NProt { get; set; }

        [XmlElement("xJust", Order = 2)]
        public string Justificativa { get; set; }

        public DetalhamentoEvento(string versao, ulong numeroProtocolo, string justificativa)
        {
            Versao = versao;
            DescricaoEvento = "Cancelamento";
            NProt = numeroProtocolo;
            Justificativa = justificativa;
        }
    }
}
