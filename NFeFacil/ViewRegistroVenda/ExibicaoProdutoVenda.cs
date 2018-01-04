using NFeFacil.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    struct ExibicaoProdutoVenda
    {
        public ProdutoSimplesVenda Base { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public string ValorUnitario => Base.ValorUnitario.ToString("C");
        public string TotalBruto => (Base.ValorUnitario * Quantidade + Base.Seguro + Base.DespesasExtras).ToString("C");
    }
}
