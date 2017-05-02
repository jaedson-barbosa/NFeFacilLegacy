namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    /// <summary>
    /// Apenas um desses "filhos" pode ser diferente de null.
    /// </summary>
    public sealed class DocumentoFiscalReferenciado
    {
        /// <summary>
        /// Referencia uma NF-e (modelo 55) emitida anteriormente, vinculada a NF-e atual, ou uma NFC-e (modelo 65).
        /// </summary>
        public string refNFe { get; set; }
        public NF1AReferenciada refNF;
        public NFProdutorRuralReferenciada refNFP;
        public CupomFiscalReferenciado refECF;
    }
}
