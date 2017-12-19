using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class FornecimentoDiario
    {
        [XmlElement("dia", Order = 0)]
        public int Dia { get; set; }

        [XmlElement("qtde", Order = 1), DescricaoPropriedade("Quantidade")]
        public double Qtde { get; set; }
    }
}
