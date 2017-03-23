namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// O preenchimento dos campos do grupo de ICMS são variáveis e dependem do CST ou do CSOSN do item de produto.
    /// Por causa do tamanho desta classe será usada uma solução do tipo "gambiarra".
    /// </summary>
    public abstract class ComumICMS
    {
        /// <summary>
        /// Origem da mercadoria.
        /// </summary>
        public int orig { get; set; }
    }
}
