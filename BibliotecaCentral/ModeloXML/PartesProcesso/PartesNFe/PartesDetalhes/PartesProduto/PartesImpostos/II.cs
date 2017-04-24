namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class II : Imposto
    {
        /// <summary>
        /// Valor BC do Imposto de Importação.
        /// </summary>
        public string vBC { get; set; }

        /// <summary>
        /// Valor despesas aduaneiras.
        /// </summary>
        public string vDespAdu { get; set; }

        /// <summary>
        /// Valor Imposto de Importação.
        /// </summary>
        public string vII { get; set; }

        /// <summary>
        /// Valor Imposto sobre Operações Financeiras.
        /// </summary>
        public string vIOF { get; set; }

        public override bool IsValido => NaoNulos(vBC, vDespAdu, vII, vIOF);
    }
}
