using NFeFacil.Controles;
using System;
using System.Collections.ObjectModel;

namespace NFeFacil.View
{
    internal interface IHambuguer
    {
        ObservableCollection<ItemHambuguer> ConteudoMenu { get; }
        int SelectedIndex { set; }
    }
}
