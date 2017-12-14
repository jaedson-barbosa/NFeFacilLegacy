using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Imposto sobre Produtos Industrializados.
    /// </summary>
    public class IPI : IImposto
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Classe de enquadramento do IPI para Cigarros e Bebidas")]
        public string clEnq { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("CNPJ do produtor da mercadoria, quando diferente do emitente")]
        public string CNPJProd { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Código do selo de controle IPI")]
        public string cSelo { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Quantidade de selos de controle")]
        public string qSelo { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Código de Enquadramento Legal do IPI")]
        public string cEnq { get; set; }

        [XmlElement(nameof(IPINT), Type = typeof(IPINT), Order = 5),
            XmlElement(nameof(IPITrib), Type = typeof(IPITrib), Order = 5)]
        public ComumIPI Corpo { get; set; }
    }
}
