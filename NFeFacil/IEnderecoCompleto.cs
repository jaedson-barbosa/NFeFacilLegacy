namespace NFeFacil
{
    public interface IEnderecoCompleto
    {
        string Bairro { get; set; }
        string CEP { get; set; }
        int CodigoMunicipio { get; set; }
        string Complemento { get; set; }
        int CPais { get; set; }
        string Logradouro { get; set; }
        string NomeMunicipio { get; set; }
        string Numero { get; set; }
        string SiglaUF { get; set; }
        string Telefone { get; set; }
        string XPais { get; set; }
    }
}