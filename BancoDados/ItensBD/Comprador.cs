using System;

namespace NFeFacil.ItensBD
{
    public sealed class Comprador
    {
        public Guid Id { get; set; }
        public Guid IdEmpresa { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime UltimaData { get; set; }
    }
}
