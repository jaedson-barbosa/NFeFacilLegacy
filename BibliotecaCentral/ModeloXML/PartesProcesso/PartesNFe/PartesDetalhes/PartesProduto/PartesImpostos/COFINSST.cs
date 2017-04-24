namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS Substituição Tributária.
    /// Só deve ser informado se o Produto for sujeito a COFINS por ST (CST = 05).
    /// </summary>
    public class COFINSST : Imposto
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

        public override bool IsValido
        {
            get
            {
                return NaoNulos(vBC, pCOFINS, qBCProd, vAliqProd, vCOFINS);
            }
        }
    }
}
