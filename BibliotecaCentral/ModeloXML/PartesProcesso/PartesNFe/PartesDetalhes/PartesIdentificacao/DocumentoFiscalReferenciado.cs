namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    public sealed class DocumentoFiscalReferenciado
    {
        /// <summary>
        /// Referencia uma NF-e (modelo 55) emitida anteriormente, vinculada a NF-e atual, ou uma NFC-e (modelo 65).
        /// </summary>
        public string refNFe;

        public NF1AReferenciada refNF;
        public NFProdutorRuralReferenciada refNFP;
        public CupomFiscalReferenciado refECF;
    }
}
