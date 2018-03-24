using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS51 : ComumICMS, IRegimeNormal
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Tributação do ICMS")]
        public string CST { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Modalidade de determinação da BC do ICMS")]
        public string modBC { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Percentual da Redução de BC")]
        public string pRedBC { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Valor da BC do ICMS")]
        public string vBC { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Alíquota do imposto")]
        public string pICMS { get; set; }

        [XmlElement(Order = 6), DescricaoPropriedade("Valor do ICMS da Operação")]
        public string vICMSOp { get; set; }

        [XmlElement(Order = 7), DescricaoPropriedade("Percentual do diferimento")]
        public string pDif { get; set; }

        [XmlElement(Order = 8), DescricaoPropriedade("Valor do ICMS Diferido")]
        public string vICMSDif { get; set; }

        [XmlElement(Order = 9), DescricaoPropriedade("Valor do ICMS")]
        public string vICMS { get; set; }
    }
}
