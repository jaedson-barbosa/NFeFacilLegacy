namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS00 : ComumICMS, IRegimeNormal
    {
        /// <summary>
        /// Tributação do ICMS.
        /// </summary>
        public string CST { get; set; }

        /// <summary>
        /// Modalidade de determinação da BC do ICMS.
        /// </summary>
        public string modBC { get; set; }

        /// <summary>
        /// Valor da BC do ICMS.
        /// </summary>
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota do imposto.
        /// </summary>
        public string pICMS { get; set; }

        /// <summary>
        /// Valor do ICMS.
        /// </summary>
        public string vICMS { get; set; }
    }
}
