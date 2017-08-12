using System;

namespace Banco.ItensBD
{
    public sealed class VeiculoDI
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; }
        public string Placa { get; set; }
        public string UF { get; set; }
        public string RNTC { get; set; }
    }
}
