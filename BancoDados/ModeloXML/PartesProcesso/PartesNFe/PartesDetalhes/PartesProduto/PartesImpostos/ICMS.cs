using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ICMS : ImpostoBase
    {
        [XmlElement(nameof(ICMS00), Type = typeof(ICMS00), Order = 0),
            XmlElement(nameof(ICMS10), Type = typeof(ICMS10), Order = 0),
            XmlElement(nameof(ICMS20), Type = typeof(ICMS20), Order = 0),
            XmlElement(nameof(ICMS30), Type = typeof(ICMS30), Order = 0),
            XmlElement(nameof(ICMS40), Type = typeof(ICMS40), Order = 0),
            XmlElement(nameof(ICMS41), Type = typeof(ICMS41), Order = 0),
            XmlElement(nameof(ICMS50), Type = typeof(ICMS50), Order = 0),
            XmlElement(nameof(ICMS51), Type = typeof(ICMS51), Order = 0),
            XmlElement(nameof(ICMS60), Type = typeof(ICMS60), Order = 0),
            XmlElement(nameof(ICMS70), Type = typeof(ICMS70), Order = 0),
            XmlElement(nameof(ICMS90), Type = typeof(ICMS90), Order = 0),
            XmlElement(nameof(ICMSSN101), Type = typeof(ICMSSN101), Order = 0),
            XmlElement(nameof(ICMSSN102), Type = typeof(ICMSSN102), Order = 0),
            XmlElement(nameof(ICMSSN201), Type = typeof(ICMSSN201), Order = 0),
            XmlElement(nameof(ICMSSN202), Type = typeof(ICMSSN202), Order = 0),
            XmlElement(nameof(ICMSSN500), Type = typeof(ICMSSN500), Order = 0),
            XmlElement(nameof(ICMSSN900), Type = typeof(ICMSSN900)),
            DescricaoPropriedade("Corpo do ICMS")]
        public ComumICMS Corpo { get; set; }
    }
}
