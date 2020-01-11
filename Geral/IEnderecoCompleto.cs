namespace BaseGeral
{
    public interface IEnderecoCompleto : IEnderecoResumo
    {
        string CEP { get; set; }
        int CodigoMunicipio { get; set; }
        string Complemento { get; set; }
        int CPais { get; set; }
        string XPais { get; set; }
    }
}