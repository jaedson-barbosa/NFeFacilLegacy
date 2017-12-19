using System;
using System.ComponentModel.DataAnnotations;

namespace NFeFacil.ItensBD
{
    public sealed class Vendedor : IStatusAtivacao
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, Obsolete]
        public long CPF { get; set; }

        public string CPFStr { get; set; }

        [Required]
        public string Endereço { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
