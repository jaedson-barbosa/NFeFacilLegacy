namespace BaseGeral
{
    public interface IEnderecoResumo
    {
        string Bairro { get; set; }
        string Logradouro { get; set; }
        string NomeMunicipio { get; set; }
        string Numero { get; set; }
        string SiglaUF { get; set; }
        string Telefone { get; set; }
    }
}