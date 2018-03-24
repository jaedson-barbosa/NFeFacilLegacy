using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public abstract class ComumCOFINS
    {
        [DescricaoPropriedade("Código de Situação Tributária da COFINS")]
        [XmlElement(Order = 0)]
        public string CST { get; set; }
    }
}
