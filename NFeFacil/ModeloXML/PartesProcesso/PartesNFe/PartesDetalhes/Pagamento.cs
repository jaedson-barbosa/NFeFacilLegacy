namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Pagamento
    {
        /// <summary>
        /// Forma de pagamento.
        /// </summary>
        public string tPag;

        /// <summary>
        /// Valor do Pagamento.
        /// </summary>
        public double vPag;

        /// <summary>
        /// (Opcional)
        /// Grupo de Cartões.
        /// </summary>
        public Cartão card;
    }

    public class Cartão
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
