// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda.DARV
{
    public struct ExibicaoProduto
    {
        public string Quantidade { get; set; }
        public string CodigoProduto { get; set; }
        public string Descricao { get; set; }
        public string ValorUnitario { get; set; }
        public string TotalBruto { get; set; }
    }
}
