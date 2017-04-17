namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
{
    public sealed class RetTrib
    {
        /// <summary>
        /// Informar o valor do PIS retido.
        /// </summary>
        public string vRetPIS { get; set; }

        /// <summary>
        /// Informar o valor COFINS do retido.
        /// </summary>
        public string vRetCOFINS { get; set; }

        /// <summary>
        /// Informar o valor do CSLL retido.
        /// </summary>
        public string vRetCSLL { get; set; }

        /// <summary>
        /// Informar o valor do BC IRRF retido.
        /// </summary>
        public string vBCIRRF { get; set; }

        /// <summary>
        /// Informar o valor do IRRF retido.
        /// </summary>
        public string vIRRF { get; set; }

        /// <summary>
        /// Informar o valor da BC da retenção da Previdência retido.
        /// </summary>
        public string vBCRetPrev { get; set; }

        /// <summary>
        /// Informar o valor da retenção da Previdência retido.
        /// </summary>
        public string vRetPrev { get; set; }
    }
}
