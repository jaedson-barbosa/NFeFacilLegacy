namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS51 : ComumICMS, IRegimeNormal
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
