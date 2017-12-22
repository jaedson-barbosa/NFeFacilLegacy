using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS00 : ComumICMS, IRegimeNormal
    {
        [DescricaoPropriedade("Tributação do ICMS")]
        [XmlElement(Order = 1)]
        public string CST { get; set; }

        [DescricaoPropriedade("Modalidade de determinação da BC do ICMS")]
        [XmlElement(Order = 2)]
        public string modBC { get; set; }

        [DescricaoPropriedade("Valor da BC do ICMS")]
        [XmlElement(Order = 3)]
        public string vBC { get; set; }

        [DescricaoPropriedade("Alíquota do imposto")]
        [XmlElement(Order = 4)]
        public string pICMS { get; set; }

        [DescricaoPropriedade("Valor do ICMS")]
        [XmlElement(Order = 5)]
        public string vICMS { get; set; }
    }
}
