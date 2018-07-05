using System;

namespace BaseGeral.ItensBD
{
    public sealed class Imagem : IUltimaData, IGuidId
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }
        public byte[] Bytes { get; set; }
    }
}
