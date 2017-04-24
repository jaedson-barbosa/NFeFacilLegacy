using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class COFINS : Imposto
    {
        [XmlElement(nameof(COFINSAliq), Type = typeof(COFINSAliq)), XmlElement(nameof(COFINSNT), Type = typeof(COFINSNT)), XmlElement(nameof(COFINSOutr), Type = typeof(COFINSOutr)), XmlElement(nameof(COFINSQtde), Type = typeof(COFINSQtde))]
        public ComumCOFINS Corpo { get; set; }

        public override bool IsValido => Corpo.ToXElement(Corpo.GetType()).HasElements;
    }
}
