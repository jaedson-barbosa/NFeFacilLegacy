using NFeFacil.Controles;
using System;
using System.Collections.ObjectModel;

namespace NFeFacil
{
    internal interface IHambuguer
    {
        ObservableCollection<ItemHambuguer> ConteudoMenu { get; }
        void AtualizarMain(int index);
    }

    internal class NewIndexEventArgs : EventArgs
    {
        public int NewIndex { get; set; }
    }
}
