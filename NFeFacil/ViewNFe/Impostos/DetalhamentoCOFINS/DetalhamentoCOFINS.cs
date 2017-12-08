using NFeFacil.ViewNFe.CaixasImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    public struct DetalhamentoCOFINS : IDetalhamentoImposto
    {
        public int CST { get; set; }
        internal TiposCalculo TipoCalculo { get; set; }
        internal TiposCalculo TipoCalculoST { get; set; }
    }
}
