namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS tributado pela alíquota.
    /// </summary>
    public class COFINSAliq : ComumCOFINS
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
        /// Valor da COFINS.
        /// </summary>
        public string vCOFINS { get; set; }
    }
}
