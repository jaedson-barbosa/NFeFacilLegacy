// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoPIS
{
    public struct Detalhamento : IDetalhamentoImposto
    {
        public int CST { get; set; }
        public PrincipaisImpostos Tipo => PrincipaisImpostos.PIS;
        internal TiposCalculo TipoCalculo { get; set; }
    }
}
