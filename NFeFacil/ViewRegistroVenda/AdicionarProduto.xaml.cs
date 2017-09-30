using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
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

        public AdicionarProduto(Guid[] produtosJaAdicionados)
        {
            InitializeComponent();

            using (var db = new AplicativoContext())
            {
                ListaCompletaProdutos = new List<ExibicaoProduto>();
                var estoque = db.Estoque.Include(x => x.Alteracoes);
                foreach (var item in db.Produtos)
                {
                    if (!produtosJaAdicionados.Contains(item.Id))
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
                }
                ListaCompletaProdutos.Sort((a, b) => a.Nome.CompareTo(b.Nome));
                if (ListaCompletaProdutos.Count == 0)
                {
                    Log.Popup.Current.Escrever(Log.TitulosComuns.Atenção, "Não existem mais produtos adicionáveis.");
                }
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

        private void VerificarConformidadeEstoque(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var log = Log.Popup.Current;
            if (ProdutoSelecionado.Base == null)
            {
                log.Escrever(Log.TitulosComuns.Atenção, "Escolha um produto.");
            }
            else if (Quantidade <= 0)
            {
                log.Escrever(Log.TitulosComuns.Atenção, "Insira uma quantidade maior que 0.");
            }
            else if (ProdutoSelecionado.Estoque != "Infinito" && Quantidade > double.Parse(ProdutoSelecionado.Estoque))
            {
                args.Cancel = true;
                log.Escrever(Log.TitulosComuns.Atenção, "A quantidade vendida não pode ser maior que a quantidade em estoque.");
            }
        }
    }
}
