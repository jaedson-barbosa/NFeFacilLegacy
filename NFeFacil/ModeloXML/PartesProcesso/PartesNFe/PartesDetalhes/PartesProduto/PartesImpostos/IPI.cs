using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Imposto sobre Produtos Industrializados.
    /// </summary>
    public class IPI : Imposto
    {
        /// <summary>
        /// (Opcional)
        /// Classe de enquadramento do IPI para Cigarros e Bebidas.
        /// </summary>
        [XmlElement(Order = 0)]
        public string clEnq { get; set; }

        /// <summary>
        /// (Opcional)
        /// CNPJ do produtor da mercadoria, quando diferente do emitente. Somente para os casos de exportação direta ou indireta.
        /// </summary>
        [XmlElement(Order = 1)]
        public string CNPJProd { get; set; }

        /// <summary>
        /// (Opcional)
        /// Código do selo de controle IPI.
        /// </summary>
        [XmlElement(Order = 2)]
        public string cSelo { get; set; }

        /// <summary>
        /// (Opcional)
        /// Quantidade de selos de controle.
        /// </summary>
        [XmlElement(Order = 3)]
        public string qSelo { get; set; }

        /// <summary>
        /// Código de Enquadramento Legal do IPI.
        /// </summary>
        [XmlElement(Order = 4)]
        public string cEnq { get; set; }

        [XmlElement(nameof(IPINT), Type = typeof(IPINT), Order = 5),
            XmlElement(nameof(IPITrib), Type = typeof(IPITrib), Order = 5)]
        public ComumIPI Corpo { get; set; }

        public override bool IsValido => Corpo != null && !string.IsNullOrEmpty(Corpo.CST)
            && !string.IsNullOrEmpty(cEnq);
    }
}
