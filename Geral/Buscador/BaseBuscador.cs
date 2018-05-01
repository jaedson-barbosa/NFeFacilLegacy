using System.Collections.ObjectModel;

namespace BaseGeral.Buscador
{
    public abstract class BaseBuscador<TipoBusca>
    {
        protected const string InvalidProduct = "#####";
        protected TipoBusca[] TodosItens { get; set; }
        public ObservableCollection<TipoBusca> Itens { get; protected set; }

        public void Buscar(string busca)
        {
            for (int i = 0; i < TodosItens.Length; i++)
            {
                try
                {
                    var atual = TodosItens[i];
                    bool valido = ItemComparado(atual, DefinicoesPermanentes.ModoBuscaProduto)
                        .ToUpper().Contains(busca.ToUpper());
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

        protected abstract string ItemComparado(TipoBusca item, int modoBusca);
        protected abstract void InvalidarItem(TipoBusca item, int modoBusca);
        public void Remover(TipoBusca produto)
        {
            var modoBusca = DefinicoesPermanentes.ModoBuscaProduto;
            for (int i = 0; i < TodosItens.Length; i++)
            {
                var at = TodosItens[i];
                if (ItemComparado(at, modoBusca) == ItemComparado(produto, modoBusca))
                    InvalidarItem(at, modoBusca);
            }
            Itens.Remove(produto);
        }
    }
}
