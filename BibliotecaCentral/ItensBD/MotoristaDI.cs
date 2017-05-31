using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;

namespace BibliotecaCentral.ItensBD
{
    public sealed class MotoristaDI
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string XEnder { get; set; }
        public string XMun { get; set; }
        public string UF { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Documento => CPF ?? CNPJ;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
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
