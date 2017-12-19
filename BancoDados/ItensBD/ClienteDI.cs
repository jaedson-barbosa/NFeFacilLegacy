using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;

namespace NFeFacil.ItensBD
{
    public sealed class ClienteDI : IStatusAtivacao
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

        public TiposDocumento TipoDocumento => (!string.IsNullOrEmpty(IdEstrangeiro)) ? TiposDocumento.idEstrangeiro :
            (!string.IsNullOrEmpty(CNPJ)) ? TiposDocumento.CNPJ : TiposDocumento.CPF;

        public ClienteDI() { }
        public ClienteDI(Destinatario other)
        {
            if (other.IndicadorIE == 9)
            {
                if (!string.IsNullOrEmpty(other.InscricaoEstadual))
                {
                    other.IndicadorIE = 1;
                }
                else
                {
                    other.IndicadorIE = 9;
                }
            }

            CPF = other.CPF;
            CNPJ = other.CNPJ;
            IdEstrangeiro = other.IdEstrangeiro;
            Nome = other.Nome;
            IndicadorIE = other.IndicadorIE;
            InscricaoEstadual = other.InscricaoEstadual;
            ISUF = other.ISUF;
            Email = other.Email;

            Logradouro = other.Endereco.Logradouro;
            Numero = other.Endereco.Numero;
            Complemento = other.Endereco.Complemento;
            Bairro = other.Endereco.Bairro;
            CodigoMunicipio = other.Endereco.CodigoMunicipio;
            NomeMunicipio = other.Endereco.NomeMunicipio;
            SiglaUF = other.Endereco.SiglaUF;
            CEP = other.Endereco.CEP;
            CPais = other.Endereco.CPais;
            XPais = other.Endereco.XPais;
            Telefone = other.Endereco.Telefone;
        }
        public ClienteDI(Emitente emit)
        {
            CNPJ = emit.CNPJ;
            Nome = emit.Nome;
            IndicadorIE = 1;
            InscricaoEstadual = emit.InscricaoEstadual;

            Logradouro = emit.Endereco.Logradouro;
            Numero = emit.Endereco.Numero;
            Complemento = emit.Endereco.Complemento;
            Bairro = emit.Endereco.Bairro;
            CodigoMunicipio = emit.Endereco.CodigoMunicipio;
            NomeMunicipio = emit.Endereco.NomeMunicipio;
            SiglaUF = emit.Endereco.SiglaUF;
            CEP = emit.Endereco.CEP;
            CPais = emit.Endereco.CPais;
            XPais = emit.Endereco.XPais;
            Telefone = emit.Endereco.Telefone;
        }

        public Destinatario ToDestinatario()
        {
            return new Destinatario
            {
                CPF = CPF,
                CNPJ = CNPJ,
                IdEstrangeiro = IdEstrangeiro,
                Nome = Nome,
                IndicadorIE = IndicadorIE,
                InscricaoEstadual = InscricaoEstadual,
                ISUF = string.IsNullOrWhiteSpace(ISUF) ? null : ISUF,
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email,
                Endereco = new ModeloXML.PartesProcesso.PartesNFe.EnderecoCompleto
                {
                    Logradouro = Logradouro,
                    Numero = Numero,
                    Complemento = string.IsNullOrWhiteSpace(Complemento) ? null : Complemento,
                    Bairro = Bairro,
                    CodigoMunicipio = CodigoMunicipio,
                    NomeMunicipio = NomeMunicipio,
                    SiglaUF = SiglaUF,
                    CEP = CEP,
                    CPais = CPais,
                    XPais = XPais,
                    Telefone = string.IsNullOrEmpty(Telefone) ? null : Telefone
                }
            };
        }
    }
}
