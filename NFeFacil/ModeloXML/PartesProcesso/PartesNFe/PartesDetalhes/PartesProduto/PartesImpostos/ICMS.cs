using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ICMS : Imposto
    {
        [XmlElement(nameof(ICMS00), Type = typeof(ICMS00)), XmlElement(nameof(ICMS10), Type = typeof(ICMS10)), XmlElement(nameof(ICMS20), Type = typeof(ICMS20)), XmlElement(nameof(ICMS30), Type = typeof(ICMS30)), XmlElement(nameof(ICMS40), Type = typeof(ICMS40)), XmlElement(nameof(ICMS41), Type = typeof(ICMS41)), XmlElement(nameof(ICMS50), Type = typeof(ICMS50)), XmlElement(nameof(ICMS51), Type = typeof(ICMS51)), XmlElement(nameof(ICMS60), Type = typeof(ICMS60)), XmlElement(nameof(ICMS70), Type = typeof(ICMS70)), XmlElement(nameof(ICMS90), Type = typeof(ICMS90)), XmlElement(nameof(ICMSPart), Type = typeof(ICMSPart)), XmlElement(nameof(ICMSSN101), Type = typeof(ICMSSN101)), XmlElement(nameof(ICMSSN102), Type = typeof(ICMSSN102)), XmlElement(nameof(ICMSSN201), Type = typeof(ICMSSN201)), XmlElement(nameof(ICMSSN202), Type = typeof(ICMSSN202)), XmlElement(nameof(ICMSSN500), Type = typeof(ICMSSN500)), XmlElement(nameof(ICMSSN900), Type = typeof(ICMSSN900))]
        public ComumICMS Corpo { get; set; }

        public override bool IsValido => Corpo.ToXElement(Corpo.GetType()).HasElements;
    }
}
