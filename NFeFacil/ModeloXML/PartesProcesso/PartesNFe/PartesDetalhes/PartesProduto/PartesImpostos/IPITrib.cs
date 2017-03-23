namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo do CST 00, 49, 50 e 99.
    /// </summary>
    public class IPITrib : ComumIPI
    {
        /// <summary>
        /// Valor da BC do IPI.
        /// </summary>
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do IPI.
        /// </summary>
        public string pIPI { get; set; }

        /// <summary>
        /// Quantidade total na unidade padrão para tributação (somente para os produtos tributados por unidade).
        /// </summary>
        public string qUnid { get; set; }

        /// <summary>
        /// Valor por Unidade Tributável.
        /// </summary>
        public string vUnid { get; set; }

        /// <summary>
        /// Valor do IPI.
        /// </summary>
        public string vIPI { get; set; }
    }
}
