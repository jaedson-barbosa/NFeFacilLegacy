using System;

namespace BaseGeral.ItensBD
{
    public sealed class FornecedorDI : IGuidId, IEnderecoResumo, IUltimaData
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Email { get; set; }

        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string NomeMunicipio { get; set; }
        public string Numero { get; set; }
        public string SiglaUF { get; set; }
        public string Telefone { get; set; }
    }
}
