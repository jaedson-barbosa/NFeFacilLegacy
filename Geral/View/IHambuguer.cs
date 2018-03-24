using BaseGeral.Controles;
using System.Collections.ObjectModel;

namespace NFeFacil.View
{
    public interface IHambuguer
    {
        ObservableCollection<ItemHambuguer> ConteudoMenu { get; }
        int SelectedIndex { set; }
    }
}
