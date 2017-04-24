namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo de COFINS tributado por quantidade
    /// </summary>
    public class COFINSQtde : ComumCOFINS
    {
        /// <summary>
        /// Quantidade Vendida.
        /// </summary>
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota da COFINS (em reais).
        /// </summary>
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor da COFINS.
        /// </summary>
        public string vCOFINS { get; set; }
    }
}
