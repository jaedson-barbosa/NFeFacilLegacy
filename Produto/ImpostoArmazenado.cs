using Produto.Impostos;

namespace Produto
{
    public class ImpostoArmazenado
    {
        public PrincipaisImpostos Tipo { get; set; }
        public int CST { get; set; }
        public string NomeTemplate { get; set; }
        public bool EdicaoAtivada { get; set; }
    }
}
