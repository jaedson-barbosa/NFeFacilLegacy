// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

using System;

namespace Venda.ViewProdutoVenda
{
    public struct ExibicaoProdutoListaGeral
    {
        public Guid Id { get; set; }
        public bool IsCabecalho { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string ValorUnitario { get; set; }
        public string TotalLiquido { get; set; }
    }
}
