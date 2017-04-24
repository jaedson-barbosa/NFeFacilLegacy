namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS30 : RegimeNormal
    {
        /// <summary>
        /// Modalidade de determinação da BC do ICMS ST.
        /// </summary>
        public string modBCST { get; set; }

        /// <summary>
        /// Percentual da margem de valor Adicionado do ICMS ST.
        /// </summary>
        public string pMVAST { get; set; }

        /// <summary>
        /// Percentual da Redução de BC do ICMS ST.
        /// </summary>
        public string pRedBCST { get; set; }

        /// <summary>
        /// Valor da BC do ICMS ST.
        /// </summary>
        public string vBCST { get; set; }

        /// <summary>
        /// Alíquota do imposto do ICMS ST.
        /// </summary>
        public string pICMSST { get; set; }

        /// <summary>
        /// Valor do ICMS ST.
        /// </summary>
        public string vICMSST { get; set; }

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
