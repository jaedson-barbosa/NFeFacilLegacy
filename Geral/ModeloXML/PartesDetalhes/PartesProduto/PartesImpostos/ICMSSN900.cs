using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN900 : ComumICMS, ISimplesNacional
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Código de Situação da Operação – Simples Nacional")]
        public string CSOSN { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Modalidade de determinação da BC do ICMS")]
        public string modBC { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor da BC do ICMS")]
        public string vBC { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Percentual da Redução de BC")]
        public string pRedBC { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Alíquota do imposto")]
        public string pICMS { get; set; }

        [XmlElement(Order = 6), DescricaoPropriedade("Valor do ICMS")]
        public string vICMS { get; set; }

        [XmlElement(Order = 7), DescricaoPropriedade("Modalidade de determinação da BC do ICMS ST")]
        public string modBCST { get; set; }

        [XmlElement(Order = 8), DescricaoPropriedade("Percentual da margem de valor Adicionado do ICMS ST")]
        public string pMVAST { get; set; }

        [XmlElement(Order = 9), DescricaoPropriedade("Percentual da Redução de BC do ICMS ST")]
        public string pRedBCST { get; set; }

        [XmlElement(Order = 10), DescricaoPropriedade("Valor da BC do ICMS ST")]
        public string vBCST { get; set; }

        [XmlElement(Order = 11), DescricaoPropriedade("Alíquota do imposto do ICMS ST")]
        public string pICMSST { get; set; }

        [XmlElement(Order = 12), DescricaoPropriedade("Valor do ICMS ST")]
        public string vICMSST { get; set; }

        [XmlElement(Order = 13), DescricaoPropriedade("Alíquota aplicável de cálculo do crédito (Simples Nacional)")]
        public string pCredSN { get; set; }

        [XmlElement(Order = 14), DescricaoPropriedade("Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)")]
        public string vCredICMSSN { get; set; }
    }
}
