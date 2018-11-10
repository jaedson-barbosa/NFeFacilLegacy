using System;
using System.ComponentModel.DataAnnotations;

namespace BaseGeral.ItensBD
{
    public sealed class NFeDI : IUltimaData, IStatusAtual
    {
        public string Id { get; set; }
        public DateTime UltimaData { get; set; }

        [Required]
        public int NumeroNota { get; set; }
        [Required]
        public ushort SerieNota { get; set; }
        [Required]
        public string NomeEmitente { get; set; }
        [Required]
        public string CNPJEmitente { get; set; }
        [Required]
        public string NomeCliente { get; set; }
        [Required]
        public string DataEmissao { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string XML { get; set; }

        public bool Impressa { get; set; }
        public bool Exportada { get; set; }

        public int StatusAdd => (int)StatusNota.Salva;
        public bool IsNFCe { get; set; }
    }
}
