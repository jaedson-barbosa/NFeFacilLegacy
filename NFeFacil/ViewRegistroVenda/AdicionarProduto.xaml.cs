using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class AdicionarProduto : ContentDialog
    {
        List<ExibicaoProduto> ListaCompletaProdutos { get; }
        public ObservableCollection<ExibicaoProduto> Produtos { get; }
        public ExibicaoProduto ProdutoSelecionado { get; set; }

        public double Quantidade { get; set; }
        public double Frete { get; set; }
        public double Seguro { get; set; }
        public double DespesasExtras { get; set; }

        public AdicionarProduto()
        {
            InitializeComponent();

            using (var db = new AplicativoContext())
            {
                ListaCompletaProdutos = new List<ExibicaoProduto>();
                var estoque = db.Estoque.Include(x => x.Alteracoes);
                foreach (var item in db.Produtos)
                {
                    var est = estoque.FirstOrDefault(x => x.Id == item.Id);
                    var quant = est != null ? est.Alteracoes.Sum(x => x.Alteração) : 0;
                    if (est == null || quant > 0)
                    {
                        var novoProd = new ExibicaoProduto
                        {
                            Base = item,
                            Codigo = item.CodigoProduto,
                            Nome = item.Descricao,
                            Estoque = est == null ? "Infinito" : quant.ToString("N"),
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

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < ListaCompletaProdutos.Count; i++)
            {
                var atual = ListaCompletaProdutos[i];
                var valido = atual.Nome.ToUpper().Contains(busca.ToUpper());
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
