using Banco.ModeloXML;
using Banco.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;

namespace Banco.ItensBD
{
    public sealed class ClienteDI
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

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Documento => CPF ?? CNPJ ?? IdEstrangeiro;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
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
                ISUF = ISUF,
                Email = Email,
                Endereco = new ModeloXML.PartesProcesso.PartesNFe.EnderecoCompleto
                {
                    Logradouro = Logradouro,
                    Numero = Numero,
                    Complemento = Complemento,
                    Bairro = Bairro,
                    CodigoMunicipio = CodigoMunicipio,
                    NomeMunicipio = NomeMunicipio,
                    SiglaUF = SiglaUF,
                    CEP = CEP,
                    CPais = CPais,
                    XPais = XPais,
                    Telefone = Telefone
                }
            };
        }
    }
}
