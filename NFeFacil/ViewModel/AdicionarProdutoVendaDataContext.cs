using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class AdicionarProdutoVendaDataContext
    {
        List<ExibicaoProduto> ListaCompletaProdutos { get; }
        public ObservableCollection<ExibicaoProduto> Produtos { get; }
        public ExibicaoProduto ProdutoSelecionado { get; set; }

        string busca;
        public string Busca
        {
            get => busca;
            set
            {
                busca = value;
                for (int i = 0; i < ListaCompletaProdutos.Count; i++)
                {
                    var atual = ListaCompletaProdutos[i];
                    var valido = atual.Nome.ToUpper().Contains(value.ToUpper());
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
                ListaCompletaProdutos = new List<ExibicaoProduto>();
                foreach (var item in db.Produtos)
                {
                    var est = db.Estoque.Find(item.Id);
                    var quant = est != null ? est.Alteracoes.Sum(x => x.Alteração) : 0;
                    if (est == null || quant > 0)
                    {
                        var novoProd = new ExibicaoProduto
                        {
                            Base = item,
                            Codigo = item.CodigoProduto,
                            Nome = item.Descricao,
                            Estoque = est == null ? "Indisponível" : quant.ToString("N"),
                            Preço = item.ValorUnitario.ToString("C"),
                            PreçoDouble = item.ValorUnitario
                        };
                        ListaCompletaProdutos.Add(novoProd);
                    }
                }
                ListaCompletaProdutos.Sort((a, b) => a.Nome.CompareTo(b.Nome));
                Produtos = ListaCompletaProdutos.GerarObs();
            }
        }

        public struct ExibicaoProduto
        {
            public ProdutoDI Base { get; set; }
            public string Codigo { get; set; }
            public string Nome { get; set; }
            public string Estoque { get; set; }
            public double PreçoDouble { get; set; }
            public string Preço { get; set; }
        }
    }
}
