using System;

namespace BaseGeral.ItensBD
{
    public sealed class CategoriaDI : IGuidId, IUltimaData
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string Nome { get; set; }
    }
}
