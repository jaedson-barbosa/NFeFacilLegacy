namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS60 : ComumICMS, IRegimeNormal
    {
        /// <summary>
        /// Tributação do ICMS.
        /// </summary>
        public string CST { get; set; }

        /// <summary>
        /// Valor da BC do ICMS Retido Anteriormente.
        /// </summary>
        public string vBCSTRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido Anteriormente.
        /// </summary>
        public string vICMSSTRet { get; set; }
    }
}
