using System.Collections.ObjectModel;

namespace BaseGeral.Buscador
{
    public abstract class BaseBuscador<TipoBusca>
    {
        protected const string InvalidProduct = "#####";
        protected TipoBusca[] TodosItens { get; set; }
        public ObservableCollection<TipoBusca> Itens { get; protected set; }
        int ModoBusca { get; }

        public BaseBuscador(int modoBusca) => ModoBusca = modoBusca;

        public void Buscar(string busca)
        {
            for (int i = 0; i < TodosItens.Length; i++)
            {
                try
                {
                    var atual = TodosItens[i];
                    busca = busca.ToUpper();
                    var comparados = ItemComparado(atual, ModoBusca);
                    bool valido = comparados.Item1.ToUpper().Contains(busca)
                        || (comparados.Item2?.ToUpper().Contains(busca) ?? false);
                    if (valido && !Itens.Contains(atual))
                    {
                        Itens.Add(atual);
                    }
                    else if (!valido && Itens.Contains(atual))
                    {
                        Itens.Remove(atual);
                    }
                }
                catch (System.Exception) { }
            }
        }

        protected abstract (string, string) ItemComparado(TipoBusca item, int modoBusca);
        protected abstract void InvalidarItem(TipoBusca item, int modoBusca);
        public void Remover(TipoBusca produto)
        {
            var modoBusca = ModoBusca;
            for (int i = 0; i < TodosItens.Length; i++)
            {
                var at = TodosItens[i];
                (string, string) comp1 = ItemComparado(at, modoBusca), comp2 = ItemComparado(produto, modoBusca);
                if (comp1.Item1 == comp2.Item1 && comp1.Item2 == comp2.Item2)
                    InvalidarItem(at, modoBusca);
            }
            Itens.Remove(produto);
        }
    }
}
