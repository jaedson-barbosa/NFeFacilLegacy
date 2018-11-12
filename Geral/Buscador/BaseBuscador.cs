using System.Collections.ObjectModel;

namespace BaseGeral.Buscador
{
    public abstract class BaseBuscador<TipoBusca>
    {
        protected const string InvalidItem = "#####";
        protected TipoBusca[] TodosItens { get; set; }
        public ObservableCollection<TipoBusca> Itens { get; protected set; }
        int ModoBusca { get; }

        public BaseBuscador(int modoBusca) => ModoBusca = modoBusca;

        string lastBusca;
        public void Buscar() => Buscar(lastBusca);
        public void Buscar(string busca)
        {
            lastBusca = busca;
            for (int i = 0; i < TodosItens.Length; i++)
            {
                var atual = TodosItens[i];
                var comparados = ItemComparado(atual, ModoBusca);
                if (string.IsNullOrEmpty(busca))
                {
                    if (comparados.Item1 == InvalidItem)
                        Itens.Remove(atual);
                    else if (!Itens.Contains(atual))
                        Itens.Add(atual);
                    continue;
                }
                try
                {
                    busca = busca.ToUpper();
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
