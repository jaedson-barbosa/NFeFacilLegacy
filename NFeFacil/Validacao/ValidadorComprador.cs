using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;

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
                new ConjuntoAnalise(Comprador.IdEmpresa == default(Guid), "Selecione uma empresa 'dona' deste comprador"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Comprador.Telefone), "Telefone não pode estar em branco"),
                new ConjuntoAnalise(string.IsNullOrWhiteSpace(Comprador.Nome), "Nome não pode estar em branco"),
                new ConjuntoAnalise(string.IsNullOrWhiteSpace(Comprador.Email), "Email não pode estar em branco"));
        }
    }
}
