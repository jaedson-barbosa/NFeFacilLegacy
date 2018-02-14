using System;

namespace NFeFacil
{
    public sealed class ErroCadastro : Exception
    {
        public ErroCadastro(string message) : base(message) { }
    }
}
