using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
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
        public string clEnq { get; set; }

        /// <summary>
        /// (Opcional)
        /// CNPJ do produtor da mercadoria, quando diferente do emitente. Somente para os casos de exportação direta ou indireta.
        /// </summary>
        public string CNPJProd { get; set; }

        /// <summary>
        /// (Opcional)
        /// Código do selo de controle IPI.
        /// </summary>
        public string cSelo { get; set; }

        /// <summary>
        /// (Opcional)
        /// Quantidade de selos de controle.
        /// </summary>
        public string qSelo { get; set; }

        /// <summary>
        /// Código de Enquadramento Legal do IPI.
        /// </summary>
        public string cEnq { get; set; }

        private ComumIPI corpo;
        [XmlElement(nameof(IPINT), Type = typeof(IPINT)), XmlElement(nameof(IPITrib), Type = typeof(IPITrib))]
        public ComumIPI Corpo
        {
            get
            {
                if (corpo == null) corpo = new IPITrib();
                return corpo;
            }
            set
            {
                corpo = value;
            }
        }

        public override bool IsValido => Corpo.ToXElement(Corpo.GetType()).HasElements;
    }
}
