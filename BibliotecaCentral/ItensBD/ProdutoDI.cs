using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace BibliotecaCentral.ItensBD
{
    public sealed class ProdutoDI : BaseProdutoOuServico, IId, IConverterDI<BaseProdutoOuServico>
    {
        public int Id { get; set; }
        public ProdutoDI() { }
        public ProdutoDI(BaseProdutoOuServico prod) : base(prod) { }

        public IId Converter(BaseProdutoOuServico item) => new ProdutoDI(item);
    }
}
