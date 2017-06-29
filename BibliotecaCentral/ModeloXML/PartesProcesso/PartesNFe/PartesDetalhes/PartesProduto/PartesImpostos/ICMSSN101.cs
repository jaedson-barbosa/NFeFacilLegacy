namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN101 : ComumICMS, ISimplesNacional
    {
        /// <summary>
        /// Código de Situação da Operação – Simples Nacional.
        /// </summary>
        public string CSOSN { get; set; }

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
