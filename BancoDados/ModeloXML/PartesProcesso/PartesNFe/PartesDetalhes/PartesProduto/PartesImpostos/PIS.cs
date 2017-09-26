using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class PIS : Imposto
    {
        [XmlElement(nameof(PISAliq), Type = typeof(PISAliq), Order = 0),
            XmlElement(nameof(PISNT), Type = typeof(PISNT), Order = 0),
            XmlElement(nameof(PISOutr), Type = typeof(PISOutr), Order = 0),
            XmlElement(nameof(PISQtde), Type = typeof(PISQtde), Order = 0),
            DescricaoPropriedade("Corpo do PIS")]
        public ComumPIS Corpo { get; set; }

        public override bool IsValido
        {
            get
            {
                if (Corpo != null)
                {
                    return Corpo.ToXElement(Corpo.GetType()).HasElements;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
