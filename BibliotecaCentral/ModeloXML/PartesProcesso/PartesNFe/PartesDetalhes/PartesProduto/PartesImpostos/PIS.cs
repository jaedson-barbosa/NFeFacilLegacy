using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class PIS : Imposto
    {
        [XmlElement(Type = typeof(PISAliq)), XmlElement(Type = typeof(PISNT)), XmlElement(Type = typeof(PISOutr)), XmlElement(Type = typeof(PISQtde))]
        public ComumPIS Corpo { get; set; }

        public override bool IsValido => Corpo.ToXElement(Corpo.GetType()).HasElements;
    }
}
