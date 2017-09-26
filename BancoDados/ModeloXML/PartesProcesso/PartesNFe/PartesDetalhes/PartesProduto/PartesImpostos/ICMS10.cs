using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS10 : ComumICMS, IRegimeNormal
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

        [DescricaoPropriedade("Modalidade de determinação da BC do ICMS ST")]
        [XmlElement(Order = 6)]
        public string modBCST { get; set; }

        [DescricaoPropriedade("Percentual da margem de valor Adicionado do ICMS ST")]
        [XmlElement(Order = 7)]
        public string pMVAST { get; set; }

        [DescricaoPropriedade("Percentual da Redução de BC do ICMS ST")]
        [XmlElement(Order = 8)]
        public string pRedBCST { get; set; }

        [DescricaoPropriedade("Valor da BC do ICMS ST")]
        [XmlElement(Order = 9)]
        public string vBCST { get; set; }

        [DescricaoPropriedade("Alíquota do imposto do ICMS ST")]
        [XmlElement(Order = 10)]
        public string pICMSST { get; set; }

        [DescricaoPropriedade("Valor do ICMS ST")]
        [XmlElement(Order = 11)]
        public string vICMSST { get; set; }
    }
}
