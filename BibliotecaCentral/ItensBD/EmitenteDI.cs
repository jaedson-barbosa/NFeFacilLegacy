using System;

namespace BibliotecaCentral.ItensBD
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

        public EmitenteDI() { }
        public EmitenteDI(ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Emitente other)
        {
            CNPJ = other.CNPJ;
            Nome = other.nome;
            NomeFantasia = other.nomeFantasia;
            InscricaoEstadual = other.inscricaoEstadual;
            IEST = other.IEST;
            IM = other.IM;
            CNAE = other.CNAE;
            RegimeTributario = other.regimeTributario;

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

        public ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Emitente ToEmitente()
        {
            return new ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Emitente
            {
                CNPJ = CNPJ,
                nome = Nome,
                nomeFantasia = NomeFantasia,
                inscricaoEstadual = InscricaoEstadual,
                IEST = IEST,
                IM = IM,
                CNAE = CNAE,
                regimeTributario = RegimeTributario,
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
