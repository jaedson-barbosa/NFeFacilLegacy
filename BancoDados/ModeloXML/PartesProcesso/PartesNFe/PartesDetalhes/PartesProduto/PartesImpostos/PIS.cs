using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class PIS : ImpostoBase
    {
        [XmlElement(nameof(PISAliq), Type = typeof(PISAliq), Order = 0),
            XmlElement(nameof(PISNT), Type = typeof(PISNT), Order = 0),
            XmlElement(nameof(PISOutr), Type = typeof(PISOutr), Order = 0),
            XmlElement(nameof(PISQtde), Type = typeof(PISQtde), Order = 0),
            DescricaoPropriedade("Corpo do PIS")]
        public ComumPIS Corpo { get; set; }
    }
}
