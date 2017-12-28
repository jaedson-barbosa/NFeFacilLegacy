using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.ViewNFe
{
    class DadosAdicaoProduto
    {
        public DadosAdicaoProduto(ProdutoDI auxiliar)
        {
            Completo = new DetalhesProdutos
            {
                Produto = auxiliar.ToProdutoOuServico()
            };
            Auxiliar = auxiliar;
        }

        public DetalhesProdutos Completo { get; }
        public ProdutoDI Auxiliar { get; }
    }
}
