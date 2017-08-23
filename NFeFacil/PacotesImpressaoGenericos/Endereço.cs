using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;

namespace NFeFacil.PacotesImpressaoGenericos
{
    public sealed class Endereço
    {
        public string UF { get; set; }
        public string Municipio { get; set; }
        public string CEP { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Fone { get; set; }

        public static implicit operator Endereço (EnderecoCompleto end)
        {
            return new Endereço
            {
                Bairro = end.Bairro,
                CEP = end.CEP,
                Fone = end.Telefone == null ? string.Empty : end.Telefone,
                Logradouro = end.Logradouro,
                Municipio = end.NomeMunicipio,
                Numero = end.Numero,
                UF = end.SiglaUF
            };
        }
    }
}
