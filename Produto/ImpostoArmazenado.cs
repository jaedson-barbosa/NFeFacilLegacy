using Venda.Impostos;

namespace Venda
{
    public class ImpostoArmazenado : IDetalhamentoImposto
    {
        public PrincipaisImpostos Tipo { get; set; }
        public int CST { get; set; }
        public string NomeTemplate { get; set; }
        public bool EdicaoAtivada { get; set; }
    }
}
