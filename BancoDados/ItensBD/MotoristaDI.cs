using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;

namespace NFeFacil.ItensBD
{
    public sealed class MotoristaDI : IStatusAtivacao
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public Guid Veiculo { get; set; }
        public string VeiculosSecundarios { get; set; }

        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string XEnder { get; set; }
        public string XMun { get; set; }
        public string UF { get; set; }

        public bool Ativo { get; set; } = true;
        public string Email { get; set; }
        public string Telefone { get; set; }

        public string Documento => CPF ?? CNPJ;

        public TiposDocumento TipoDocumento => !string.IsNullOrEmpty(CNPJ) ? TiposDocumento.CNPJ : TiposDocumento.CPF;

        public MotoristaDI() { }
        public MotoristaDI(Motorista other)
        {
            CPF = other.CPF;
            CNPJ = other.CNPJ;
            Nome = other.Nome;
            InscricaoEstadual = other.InscricaoEstadual;
            XEnder = other.XEnder;
            XMun = other.XMun;
            UF = other.UF;
        }

        public Motorista ToMotorista()
        {
            return new Motorista
            {
                CPF = CPF,
                CNPJ = CNPJ,
                Nome = Nome,
                InscricaoEstadual = InscricaoEstadual,
                XEnder = XEnder,
                XMun = XMun,
                UF = UF
            };
        }
    }
}
