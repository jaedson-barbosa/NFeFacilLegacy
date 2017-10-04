using System;
using System.Collections;

namespace NFeFacil
{
    internal interface IHambuguer
    {
        IEnumerable ConteudoMenu { get; }
        void AtualizarMain(int index);
    }

    internal class NewIndexEventArgs : EventArgs
    {
        public int NewIndex { get; set; }
    }
}
