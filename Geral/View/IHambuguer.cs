using BaseGeral.Controles;
using System.Collections.ObjectModel;

namespace BaseGeral.View
{
    public interface IHambuguer
    {
        ObservableCollection<ItemHambuguer> ConteudoMenu { get; }
        int SelectedIndex { set; }
    }
}
