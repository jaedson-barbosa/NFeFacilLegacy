using System.Xml;
using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes.PartesEnvEvento
{
    public struct DetalhamentoEvento
    {
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("descEvento")]
        public string DescEvento { get; set; }

        [XmlElement("nProt")]
        public ulong NProt { get; set; }

        [XmlElement("xJust")]
        public string XJust { get; set; }

        public DetalhamentoEvento(string versao, ulong numeroProtocolo, string justificativa)
        {
            Versao = versao;
            DescEvento = "Cancelamento";
            NProt = numeroProtocolo;
            XJust = justificativa;
        }
    }
}
