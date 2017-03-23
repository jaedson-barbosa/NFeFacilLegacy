namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Combustivel
    {
        /// <summary>
        /// Código de produto da ANP.
        /// Utilizar a codificação de produtos do SIMP.
        /// </summary>
        public ulong cProdANP { get; set; }

        /// <summary>
        /// (Opcional)
        /// Percentual de Gás Natural para o produto GLP.
        /// </summary>
        public string pMixGN { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar apenas quando a UF utilizar o CODIF
        /// (Sistema de Controle do Diferimento do Imposto nas Operações com AEAC - Álcool Etílico Anidro Combustível).
        /// </summary>
        public string CODIF { get; set; }

        /// <summary>
        /// (Opcional)
        /// Quantidade de combustível faturada à temperatura ambiente.
        /// </summary>
        public string qTemp { get; set; }

        /// <summary>
        /// Informar a UF de consumo. Informar "EX" para Exterior.
        /// </summary>
        public string UFCons { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo de informações da CIDE.
        /// </summary>
        public CIDE CIDE { get; set; } = new CIDE();
    }
}
