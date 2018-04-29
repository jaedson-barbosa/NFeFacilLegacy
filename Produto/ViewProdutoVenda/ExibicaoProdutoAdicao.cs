using BaseGeral.ItensBD;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public sealed class ExibicaoProdutoAdicao
    {
        public ProdutoDI Base { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Estoque { get; set; }
        public double PrecoDouble { get; set; }
        public string Preco { get; set; }
    }
}
