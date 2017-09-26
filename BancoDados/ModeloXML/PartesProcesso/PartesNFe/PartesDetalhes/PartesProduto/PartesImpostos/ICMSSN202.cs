using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN202 : ComumICMS, ISimplesNacional
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Código de Situação da Operação – Simples Nacional")]
        public string CSOSN { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Modalidade de determinação da BC do ICMS ST")]
        public string modBCST { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Percentual da margem de valor Adicionado do ICMS ST")]
        public string pMVAST { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Percentual da Redução de BC do ICMS ST")]
        public string pRedBCST { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Valor da BC do ICMS ST")]
        public string vBCST { get; set; }

        [XmlElement(Order = 6), DescricaoPropriedade("Alíquota do imposto do ICMS ST")]
        public string pICMSST { get; set; }

        [XmlElement(Order = 7), DescricaoPropriedade("Valor do ICMS ST")]
        public string vICMSST { get; set; }
    }
}
