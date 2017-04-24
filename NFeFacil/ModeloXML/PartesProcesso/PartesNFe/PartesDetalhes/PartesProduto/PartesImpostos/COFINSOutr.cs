namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS Outras Operações.
    /// </summary>
    public class COFINSOutr : ComumCOFINS
    {
        /// <summary>
        /// Valor da Base de Cálculo da COFINS.
        /// </summary>
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota da COFINS (em percentual).
        /// </summary>
        public string pCOFINS { get; set; }

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
