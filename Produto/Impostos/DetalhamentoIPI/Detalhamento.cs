// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoIPI
{
    public struct Detalhamento : IDetalhamentoImposto
    {
        public int CST { get; set; }
        public PrincipaisImpostos Tipo => PrincipaisImpostos.IPI;
        internal TiposCalculo TipoCalculo { get; set; }
    }
}
