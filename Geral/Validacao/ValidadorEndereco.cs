namespace BaseGeral.Validacao
{
    public sealed class ValidadorEndereco : IValidavel
    {
        private IEnderecoResumo End;

        public ValidadorEndereco(IEnderecoResumo end)
        {
            End = end;
        }

        public bool Validar(bool exibirMensagem)
        {
            return new ValidarDados().ValidarTudo(exibirMensagem,
                (string.IsNullOrEmpty(End.SiglaUF), "Não foi escolhida uma UF"),
                (string.IsNullOrEmpty(End.NomeMunicipio), "Não foi selecionado um município"),
                (string.IsNullOrEmpty(End.Logradouro), "Não foi informado o logradouro"),
                (string.IsNullOrEmpty(End.Numero), "Não foi informado o número do endereco"),
                (string.IsNullOrEmpty(End.Bairro), "Não foi informado o bairro"));
        }
    }
}
