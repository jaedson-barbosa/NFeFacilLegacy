// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public struct ExibicaoProdutoListaGeral
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public string ValorUnitario { get; set; }
        public string TotalLiquido { get; set; }
    }
}
