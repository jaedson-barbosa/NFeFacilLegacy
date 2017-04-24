namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN900 : SimplesNacional
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
        /// Valor da BC do ICMS ST da UF destino.
        /// </summary>
        public string vBCSTDest { get; set; }

        /// <summary>
        /// Valor do ICMS ST da UF destino.
        /// </summary>
        public string vICMSSTDest { get; set; }

        /// <summary>
        /// Alíquota aplicável de cálculo do crédito (Simples Nacional).
        /// </summary>
        public string pCredSN { get; set; }

        /// <summary>
        /// Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL).
        /// </summary>
        public string vCredICMSSN { get; set; }
    }
}
