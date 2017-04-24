namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS tributado por quantidade.
    /// </summary>
    public class PISQtde : ComumPIS
    {
        /// <summary>
        /// Quantidade Vendida.
        /// </summary>
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota do PIS (em reais).
        /// </summary>
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor do PIS.
        /// </summary>
        public string vPIS { get; set; }
    }
}
