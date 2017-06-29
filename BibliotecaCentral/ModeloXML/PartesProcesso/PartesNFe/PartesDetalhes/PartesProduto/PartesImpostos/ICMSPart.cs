namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo de Partilha do ICMS entre a UF de origem e UF de destino ou a UF definida na legislação. 
    /// </summary>
    public class ICMSPart : ComumICMS, IRegimeNormal
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
        /// Percentual da Redução de BC.
        /// </summary>
        public string pRedBC { get; set; }

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
        /// UF para qual é devido o ICMS ST.
        /// </summary>
        public string UFST { get; set; }

        /// <summary>
        /// Percentual da BC operação própria.
        /// </summary>
        public string pBCOp { get; set; }
    }
}
