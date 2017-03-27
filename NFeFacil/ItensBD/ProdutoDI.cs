using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ItensBD
{
    public sealed class ProdutoDI : DadosBaseProdutoOuServico
    {
        public int Id { get; set; }
        public ProdutoDI() { }
        public ProdutoDI(DadosBaseProdutoOuServico prod) : base(prod) { }
    }
}
