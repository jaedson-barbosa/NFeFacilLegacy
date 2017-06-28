namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
{
    public sealed class RetTrib
    {
        /// <summary>
        /// Informar o valor do PIS retido.
        /// </summary>
        public double vRetPIS { get; set; }

        /// <summary>
        /// Informar o valor COFINS do retido.
        /// </summary>
        public double vRetCOFINS { get; set; }

        /// <summary>
        /// Informar o valor do CSLL retido.
        /// </summary>
        public double vRetCSLL { get; set; }

        /// <summary>
        /// Informar o valor do BC IRRF retido.
        /// </summary>
        public double vBCIRRF { get; set; }

        /// <summary>
        /// Informar o valor do IRRF retido.
        /// </summary>
        public double vIRRF { get; set; }

        /// <summary>
        /// Informar o valor da BC da retenção da Previdência retido.
        /// </summary>
        public double vBCRetPrev { get; set; }

        /// <summary>
        /// Informar o valor da retenção da Previdência retido.
        /// </summary>
        public double vRetPrev { get; set; }
    }
}
