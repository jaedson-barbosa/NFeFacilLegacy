using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Banco.ItensBD
{
    public sealed class NFeDI
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
    }
}
