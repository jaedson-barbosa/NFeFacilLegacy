using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;

namespace BibliotecaCentral.ItensBD
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
            if (other.indicadorIE == 9)
            {
                if (!string.IsNullOrEmpty(other.inscricaoEstadual))
                {
                    other.indicadorIE = 1;
                }
                else
                {
                    other.indicadorIE = 9;
                }
            }

            CPF = other.CPF;
            CNPJ = other.CNPJ;
            IdEstrangeiro = other.idEstrangeiro;
            Nome = other.nome;
            IndicadorIE = other.indicadorIE;
            InscricaoEstadual = other.inscricaoEstadual;
            ISUF = other.ISUF;
            Email = other.email;

            Logradouro = other.endereco.Logradouro;
            Numero = other.endereco.Numero;
            Complemento = other.endereco.Complemento;
            Bairro = other.endereco.Bairro;
            CodigoMunicipio = other.endereco.CodigoMunicipio;
            NomeMunicipio = other.endereco.NomeMunicipio;
            SiglaUF = other.endereco.SiglaUF;
            CEP = other.endereco.CEP;
            CPais = other.endereco.CPais;
            XPais = other.endereco.XPais;
            Telefone = other.endereco.Telefone;
        }

        public Destinatario ToDestinatario()
        {
            return new Destinatario
            {
                CPF = CPF,
                CNPJ = CNPJ,
                idEstrangeiro = IdEstrangeiro,
                nome = Nome,
                indicadorIE = IndicadorIE,
                inscricaoEstadual = InscricaoEstadual,
                ISUF = ISUF,
                email = Email,
                endereco = new ModeloXML.PartesProcesso.PartesNFe.enderecoCompleto
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
