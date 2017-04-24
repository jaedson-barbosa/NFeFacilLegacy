namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class CIDE
    {
        /// <summary>
        /// Informar a BC da CIDE em quantidade.
        /// </summary>
        public double qBCProd { get; set; }

        /// <summary>
        /// Valor da alíquota da CIDE.
        /// </summary>
        public double vAliqProd { get; set; }

        /// <summary>
        /// Valor da CIDE.
        /// </summary>
        public double vCIDE { get; set; }
    }
}