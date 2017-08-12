using System;

namespace Banco.ItensBD
{
    public sealed class EmitenteDI
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string IEST { get; set; }
        public string IM { get; set; }
        public string CNAE { get; set; }
        public int RegimeTributario { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int CodigoMunicipio { get; set; }
        public string NomeMunicipio { get; set; }
        public string SiglaUF { get; set; }
        public string CEP { get; set; }
        public int CPais { get; set; } = 1058;
        public string XPais { get; set; } = "Brasil";
        public string Telefone { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is EmitenteDI emit)
            {
                return CNPJ == emit.CNPJ;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return CNPJ.GetHashCode();
        }
    }
}
