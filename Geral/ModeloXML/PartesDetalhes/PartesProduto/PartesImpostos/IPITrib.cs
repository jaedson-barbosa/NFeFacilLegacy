using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo do CST 00, 49, 50 e 99.
    /// </summary>
    public sealed class IPITrib : ComumIPI
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Valor da BC do IPI")]
        public string vBC { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Alíquota do IPI")]
        public string pIPI { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Quantidade total na unidade padrão para tributação")]
        public string qUnid { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Valor por Unidade Tributável")]
        public string vUnid { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Valor do IPI")]
        public string vIPI { get; set; }
    }
}
