using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Arma
    {
        [XmlElement("tpArma", Order = 0), DescricaoPropriedade("Indicador do tipo de arma de fogo")]
        public ushort TpArma { get; set; }

        [XmlElement("nSerie", Order = 1), DescricaoPropriedade("Número de série da arma")]
        public string NSerie { get; set; }

        [XmlElement("nCano", Order = 2), DescricaoPropriedade("Número de série do cano")]
        public string NCano { get; set; }

        [XmlElement("descr", Order = 3), DescricaoPropriedade("Descrição completa da arma")]
        public string Descr { get; set; }
    }
}
