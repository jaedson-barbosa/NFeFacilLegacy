using System.ComponentModel.DataAnnotations;

namespace NFeFacil.ItensBD
{
    public sealed class NFeDI
    {
        public int Id { get; set; }
        [Required]
        public string IdentificacaoNota { get; set; }
        [Required]
        public string NumeroNota { get; set; }
        [Required]
        public string NomeEmitente { get; set; }
        [Required]
        public string NomeCliente { get; set; }
        [Required]
        public string DataEmissao { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
