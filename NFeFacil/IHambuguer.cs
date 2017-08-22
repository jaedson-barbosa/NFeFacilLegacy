using System;
using System.Collections;

namespace NFeFacil
{
    internal interface IHambuguer
    {
        IEnumerable ConteudoMenu { get; }
        void AtualizarMain(int index);
        event EventHandler MainMudou;
    }

    internal class NewIndexEventArgs : EventArgs
    {
        public int NewIndex { get; set; }
    }
}
