// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos
{
    public interface IDetalhamentoImposto
    {
        PrincipaisImpostos Tipo { get; }
    }
}
