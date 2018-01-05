using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class II : ImpostoBase
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Valor BC do Imposto de Importação")]
        public string vBC { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Valor despesas aduaneiras")]
        public string vDespAdu { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Valor Imposto de Importação")]
        public string vII { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Valor Imposto sobre Operações Financeiras")]
        public string vIOF { get; set; }
    }
}
