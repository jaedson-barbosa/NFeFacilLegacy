using NFeFacil.ItensBD;
using NFeFacil.Log;

namespace NFeFacil.Validacao
{
    struct ValidadorComprador : IValidavel
    {
        Comprador Comprador;

        public ValidadorComprador(Comprador comprador)
        {
            Comprador = comprador;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Comprador.Telefone), "Telefone não pode estar em branco"),
                new ConjuntoAnalise(string.IsNullOrWhiteSpace(Comprador.Nome), "Nome não pode estar em branco"),
                new ConjuntoAnalise(string.IsNullOrWhiteSpace(Comprador.Email), "Email não pode estar em branco"));
        }
    }
}
