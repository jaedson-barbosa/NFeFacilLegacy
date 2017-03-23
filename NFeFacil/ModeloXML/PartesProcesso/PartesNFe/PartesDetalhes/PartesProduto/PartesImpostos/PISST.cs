namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo PIS Substituição Tributária.
    /// </summary>
    public class PISST : Imposto
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

        public override bool IsValido
        {
            get
            {
                return NaoNulos(vBC, pPIS, qBCProd, vAliqProd, vPIS);
            }
        }
    }
}
