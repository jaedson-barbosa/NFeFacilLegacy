using NFeFacil.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda.DARV
{
    public struct DadosImpressaoDARV
    {
        public RegistroVenda Venda { get; set; }
        public Dimensoes Dimensoes { get; set; }
        public ClienteDI Cliente { get; set; }
        public MotoristaDI Motorista { get; set; }
        public Vendedor Vendedor { get; set; }
        public Comprador Comprador { get; set; }
        public ProdutoDI[] ProdutosCompletos { get; set; }
    }
}
