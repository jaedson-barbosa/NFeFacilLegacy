using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Imposto sobre Produtos Industrializados.
    /// </summary>
    public class IPI : ImpostoBase
    {
        [XmlElement(Order = 0), DescricaoPropriedade("CNPJ do produtor da mercadoria, quando diferente do emitente")]
        public string CNPJProd { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Código do selo de controle IPI")]
        public string cSelo { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Quantidade de selos de controle")]
        public string qSelo { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Código de Enquadramento Legal do IPI")]
        public string cEnq { get; set; }

        [XmlElement(nameof(IPINT), Type = typeof(IPINT), Order = 4),
            XmlElement(nameof(IPITrib), Type = typeof(IPITrib), Order = 4)]
        public ComumIPI Corpo { get; set; }
    }
}
