using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;

namespace NFeFacil.Validacao
{
    internal sealed class ValidadorEndereco : IValidavel
    {
        private EnderecoCompleto End;

        public ValidadorEndereco(EnderecoCompleto end)
        {
            End = end;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(End.SiglaUF), "Não foi escolhida uma UF"),
                new ConjuntoAnalise(string.IsNullOrEmpty(End.NomeMunicipio), "Não foi selecionado um município"),
                new ConjuntoAnalise(string.IsNullOrEmpty(End.Logradouro), "Não foi informado o logradouro"),
                new ConjuntoAnalise(string.IsNullOrEmpty(End.Numero), "Não foi informado o número do endereço"),
                new ConjuntoAnalise(string.IsNullOrEmpty(End.Bairro), "Não foi informado o bairro"));
        }
    }
}
