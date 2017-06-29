namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo Tributação ICMS = 40, 41, 50.
    /// </summary>
    public class ICMS40 : ComumICMS, IRegimeNormal
    {
        /// <summary>
        /// Tributação do ICMS.
        /// </summary>
        public string CST { get; set; }

        /// <summary>
        /// Motivo da desoneração do ICMS.
        /// </summary>
        public string motDesICMS { get; set; }

        /// <summary>
        /// Valor do ICMS da desoneração.
        /// </summary>
        public string vICMSDeson { get; set; }
    }
}
