// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoICMSUFDest
{
    public struct Detalhamento : IDetalhamentoImposto
    {
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMSUFDest;
    }
}
