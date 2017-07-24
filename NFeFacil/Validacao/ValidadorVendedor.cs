using NFeFacil.Log;
using NFeFacil.ItensBD;

namespace NFeFacil.Validacao
{
    public struct ValidadorVendedor : IValidavel
    {
        Vendedor vendedor;

        public ValidadorVendedor(Vendedor vendedor)
        {
            this.vendedor = vendedor;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(vendedor.CPF == 0, "CPF inválido"),
                new ConjuntoAnalise(string.IsNullOrWhiteSpace(vendedor.Nome), "Nome não pode estar em branco"),
                new ConjuntoAnalise(string.IsNullOrWhiteSpace(vendedor.Endereço), "Endereço não pode estar em branco"));
        }
    }
}
