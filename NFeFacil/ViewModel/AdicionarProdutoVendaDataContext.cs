using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using System.Collections.ObjectModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class AdicionarProdutoVendaDataContext
    {
        ExibicaoProduto[] ListaCompletaProdutos { get; }
        public ObservableCollection<ExibicaoProduto> Produtos { get; }
        public ExibicaoProduto ProdutoSelecionado { get; set; }

        string busca;
        public string Busca
        {
            get => busca;
            set
            {
                busca = value;
                for (int i = 0; i < ListaCompletaProdutos.Length; i++)
                {
                    var atual = ListaCompletaProdutos[i];
                    var valido = atual.Nome.Contains(value);
                    if (valido && !Produtos.Contains(atual))
                    {
                        Produtos.Add(atual);
                    }
                    else if (!valido && Produtos.Contains(atual))
                    {
                        Produtos.Remove(atual);
                    }
                }
            }
        }

        public double Quantidade { get; set; }
        public double Frete { get; set; }
        public double Seguro { get; set; }
        public double DespesasExtras { get; set; }

        public AdicionarProdutoVendaDataContext()
        {
            using (var db = new AplicativoContext())
            {
                ListaCompletaProdutos = (from prod in db.Produtos
                                         let est = db.Estoque.Find(prod.Id)
                                         let quant = est.Alteracoes.Sum(x => x.Alteração)
                                         where est == null || quant > 0
                                         orderby prod.Descricao ascending
                                         select new ExibicaoProduto
                                         {
                                             Base = prod,
                                             Codigo = prod.CodigoProduto,
                                             Nome = prod.Descricao,
                                             Estoque = est == null ? "Indisponível" : quant.ToString("N"),
                                             Preço = prod.ValorUnitario.ToString("C")
                                         }).ToArray();
                Produtos = ListaCompletaProdutos.GerarObs();
            }
        }


        public struct ExibicaoProduto
        {
            public ProdutoDI Base { get; set; }
            public string Codigo { get; set; }
            public string Nome { get; set; }
            public string Estoque { get; set; }
            public string Preço { get; set; }
        }
    }
}
