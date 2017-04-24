namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS tributado pela alíquota.
    /// </summary>
    public class PISAliq : ComumPIS
    {
        /// <summary>
        /// Valor da Base de Cálculo do PIS.
        /// </summary>
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do PIS (em percentual).
        /// </summary>
        public string pPIS { get; set; }

        /// <summary>
        /// Valor do PIS.
        /// </summary>
        public string vPIS { get; set; }
    }
}
