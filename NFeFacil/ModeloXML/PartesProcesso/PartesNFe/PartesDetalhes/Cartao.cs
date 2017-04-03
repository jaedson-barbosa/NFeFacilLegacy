namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Cartao
    {
        public string CNPJ;

        /// <summary>
        /// Bandeira da operadora de cartão de crédito e/ou débito.
        /// </summary>
        public string tBand;

        /// <summary>
        /// Número de autorização da operação cartão de crédito e/ou débito.
        /// </summary>
        public string cAut;
    }
}
