using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class COFINS : Imposto
    {
        [XmlElement(nameof(COFINSAliq), Type = typeof(COFINSAliq), Order = 0),
            XmlElement(nameof(COFINSNT), Type = typeof(COFINSNT), Order = 0),
            XmlElement(nameof(COFINSOutr), Type = typeof(COFINSOutr), Order = 0),
            XmlElement(nameof(COFINSQtde), Type = typeof(COFINSQtde), Order = 0),
            DescricaoPropriedade("Corpo do COFINS")]
        public ComumCOFINS Corpo { get; set; }

        public override bool IsValido => Corpo.ToXElement(Corpo.GetType()).HasElements;
    }
}
