using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes
{
    public sealed class FornecimentoDiario
    {
        [XmlElement("dia", Order = 0)]
        public int Dia { get; set; }

        [XmlElement("qtde", Order = 1), DescricaoPropriedade("Quantidade")]
        public double Qtde { get; set; }
    }
}
