using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.Produto.Impostos;

namespace NFeFacil.Produto
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
            ImpostosPadrao = auxiliar.GetImpostosPadrao();
        }

        public DadosAdicaoProduto(ProdutoDI auxiliar, DetalhesProdutos completo)
        {
            Completo = completo;
            Auxiliar = auxiliar;
            ImpostosPadrao = auxiliar.GetImpostosPadrao();
        }

        public DetalhesProdutos Completo { get; }
        public ProdutoDI Auxiliar { get; }
        public (PrincipaisImpostos Tipo, string NomeTemplate, int CST)[] ImpostosPadrao { get; }
    }
}
