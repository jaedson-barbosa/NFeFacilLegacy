using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Medicamento
    {
        [XmlElement("nLote", Order = 0), DescricaoPropriedade("Número do Lote")]
        public string NLote { get; set; }

        [XmlElement("qLote", Order = 1), DescricaoPropriedade("Quantidade de produtos")]
        public double QLote { get; set; }

        [XmlElement("dFab", Order = 2), DescricaoPropriedade("Data de fabricação")]
        public string DFab { get; set; }

        [XmlElement("dVal", Order = 3), DescricaoPropriedade("Data de validade")]
        public string DVal { get; set; }

        [XmlElement("vPMC", Order = 4), DescricaoPropriedade("Preço máximo consumidor")]
        public double VPMC { get; set; }
    }
}
