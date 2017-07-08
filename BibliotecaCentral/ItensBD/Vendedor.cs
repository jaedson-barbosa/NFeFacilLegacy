using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaCentral.ItensBD
{
    public sealed class Vendedor
    {
        public Guid Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public long CPF { get; set; }

        [Required]
        public string Endereço { get; set; }
    }
}
