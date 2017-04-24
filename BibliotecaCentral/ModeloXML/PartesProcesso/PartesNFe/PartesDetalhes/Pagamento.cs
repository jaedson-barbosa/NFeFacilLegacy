namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Pagamento
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
        public Cartao card;
    }
}
