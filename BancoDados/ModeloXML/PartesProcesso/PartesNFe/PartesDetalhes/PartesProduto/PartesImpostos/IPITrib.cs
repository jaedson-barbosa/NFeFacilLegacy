using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo do CST 00, 49, 50 e 99.
    /// </summary>
    public sealed class IPITrib : ComumIPI
    {
        /// <summary>
        /// Valor da BC do IPI.
        /// </summary>
        [XmlElement(Order = 1)]
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do IPI.
        /// </summary>
        [XmlElement(Order = 2)]
        public string pIPI { get; set; }

        /// <summary>
        /// Quantidade total na unidade padrão para tributação (somente para os produtos tributados por unidade).
        /// </summary>
        [XmlElement(Order = 3)]
        public string qUnid { get; set; }

        /// <summary>
        /// Valor por Unidade Tributável.
        /// </summary>
        [XmlElement(Order = 4)]
        public string vUnid { get; set; }

        /// <summary>
        /// Valor do IPI.
        /// </summary>
        [XmlElement(Order = 5)]
        public string vIPI { get; set; }
    }
}
