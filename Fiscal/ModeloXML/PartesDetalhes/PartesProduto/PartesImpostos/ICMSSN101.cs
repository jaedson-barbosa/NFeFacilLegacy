using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN101 : ComumICMS, ISimplesNacional
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Código de Situação da Operação – Simples Nacional")]
        public string CSOSN { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Alíquota aplicável de cálculo do crédito (Simples Nacional)")]
        public string pCredSN { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)")]
        public string vCredICMSSN { get; set; }
    }
}
