using NFeFacil.ViewNFe.CaixasImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public struct DetalhamentoIPI : IDetalhamentoImposto
    {
        public string CST { get; set; }
        internal TiposCalculo TipoCalculo { get; set; }
    }
}
