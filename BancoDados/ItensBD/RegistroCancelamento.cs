﻿using System.ComponentModel.DataAnnotations;

namespace Banco.ItensBD
{
    public sealed class RegistroCancelamento
    {
        [Key]
        public string ChaveNFe { get; set; }
        public int TipoAmbiente { get; set; }
        public string DataHoraEvento { get; set; }
        public string XML { get; set; }
    }
}
