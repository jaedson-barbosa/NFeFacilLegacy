using System;

namespace BaseGeral.ItensBD
{
    public sealed class ClienteDI : IStatusAtivacao, IGuidId
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string IdEstrangeiro { get; set; }
        public string Nome { get; set; }
        public int IndicadorIE { get; set; }
        public string InscricaoEstadual { get; set; }
        public string ISUF { get; set; }
        public string Email { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int CodigoMunicipio { get; set; }
        public string NomeMunicipio { get; set; }
        public string SiglaUF { get; set; }
        public string CEP { get; set; }
        public int CPais { get; set; }
        public string XPais { get; set; }
        public string Telefone { get; set; }

        public bool Ativo { get; set; } = true;

        public string Documento => CPF ?? CNPJ ?? IdEstrangeiro;
    }
}
