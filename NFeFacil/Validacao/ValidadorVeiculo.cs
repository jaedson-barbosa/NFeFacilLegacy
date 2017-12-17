using NFeFacil.ItensBD;
using NFeFacil.Log;

namespace NFeFacil.Validacao
{
    public sealed class ValidadorVeiculo : IValidavel
    {
        VeiculoDI Veiculo { get; }

        public ValidadorVeiculo(VeiculoDI veiculo)
        {
            Veiculo = veiculo;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Veiculo.Descricao), "Informe uma breve descrição do veículo."),
                new ConjuntoAnalise(string.IsNullOrEmpty(Veiculo.Placa), "Informe a placa do veículo"),
                new ConjuntoAnalise(Veiculo.Placa.Contains("&"), "O caractere & não pode ser usado."),
                new ConjuntoAnalise(string.IsNullOrEmpty(Veiculo.UF), "Informe a UF da placa do veículo"));
        }
    }
}
