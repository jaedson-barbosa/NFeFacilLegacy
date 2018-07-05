// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoISSQN
{
    public struct Detalhamento : IDetalhamentoImposto
    {
        public bool Exterior { get; set; }
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ISSQN;
    }
}
