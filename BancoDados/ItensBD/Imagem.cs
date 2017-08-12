using System;
using System.Linq;
using System.Threading.Tasks;

namespace Banco.ItensBD
{
    public sealed class Imagem
    {
        public Guid Id { get; set; }
        public byte[] Bytes { get; set; }
    }
}
