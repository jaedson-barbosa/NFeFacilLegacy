using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN500 : ComumICMS, ISimplesNacional
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Código de Situação da Operação – Simples Nacional")]
        public string CSOSN { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Valor da BC do ICMS Retido Anteriormente")]
        public string vBCSTRet { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor do ICMS Retido Anteriormente")]
        public string vICMSSTRet { get; set; }
    }
}
