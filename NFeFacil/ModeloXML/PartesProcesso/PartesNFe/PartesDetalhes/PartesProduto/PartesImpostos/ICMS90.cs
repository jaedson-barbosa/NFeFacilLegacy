namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS90 : RegimeNormal
    {
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

        /// <summary>
        /// Valor da BC do ICMS Retido Anteriormente.
        /// </summary>
        public string vBCSTRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido Anteriormente.
        /// </summary>
        public string vICMSSTRet { get; set; }

        /// <summary>
        /// Motivo da desoneração do ICMS.
        /// </summary>
        public string motDesICMS { get; set; }

        /// <summary>
        /// Valor do ICMS da desoneração.
        /// </summary>
        public string vICMSDeson { get; set; }

        /// <summary>
        /// Valor do ICMS da Operação.
        /// </summary>
        public string vICMSOp { get; set; }

        /// <summary>
        /// Percentual do diferimento.
        /// </summary>
        public string pDif { get; set; }

        /// <summary>
        /// Valor do ICMS Diferido.
        /// </summary>
        public string vICMSDif { get; set; }
    }
}
