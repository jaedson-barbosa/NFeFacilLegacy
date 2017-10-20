using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ControleEstoque : Page
    {
        public ControleEstoque()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var lista = (from est in db.Estoque.Include(x => x.Alteracoes).ToArray()
                             join prod in db.Produtos.ToArray() on est.Id equals prod.Id
                             orderby prod.Descricao
                             select new Conjunto() { Produto = prod, Estoque = est }).GerarObs();
                cmbProduto.ItemsSource = lista;
                if (lista.Count > 0)
                {
                    cmbProduto.SelectedIndex = 0;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Manage, "Controle de estoque");
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                if (dadosEstoque.DataContext != null)
                {
                    var antigo = (Estoque)dadosEstoque.DataContext;
                    db.Update(antigo);
                    db.SaveChanges();
                }
            }
            base.OnNavigatingFrom(e);
        }

        private void cmbProduto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var conjAdicionado = (Conjunto)e.AddedItems[0];
            if (e.RemovedItems.Count == 0 || !conjAdicionado.Equals((Conjunto)e.RemovedItems[0]))
            {
                if (dadosEstoque.DataContext != null)
                {
                    using (var db = new AplicativoContext())
                    {
                        var antigo = (Estoque)dadosEstoque.DataContext;
                        db.Update(antigo);
                        db.SaveChanges();
                    }
                }
                else
                {
                    dadosEstoque.IsEnabled = true;
                }
                dadosEstoque.DataContext = conjAdicionado.Estoque;
                var alteracoes = conjAdicionado.Estoque.Alteracoes;
                if (alteracoes != null)
                {
                    var valores = new double[alteracoes.Count + 1];
                    valores[0] = 0;
                    for (int i = 1; i < valores.Length; i++)
                    {
                        valores[i] = alteracoes.Take(i).Sum(x => x.Alteração);
                    }
                    serieGrafico.Values = new LiveCharts.ChartValues<double>(valores);
                }
                else
                {
                    serieGrafico.Values = new LiveCharts.ChartValues<double>(new double[1] { 0 });
                }
            }
        }

        struct Conjunto
        {
            public ProdutoDI Produto { get; set; }
            public Estoque Estoque { get; set; }
        }

        private async void AlterarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarAlteracaoEstoque();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var valor = caixa.ValorProcessado;
                if (valor != 0)
                {
                    var estoque = (Estoque)dadosEstoque.DataContext;
                    estoque.UltimaData = DateTime.Now;
                    var alt = new AlteracaoEstoque() { Alteração = valor };
                    if (estoque.Alteracoes == null)
                    {
                        estoque.Alteracoes = new List<AlteracaoEstoque>() { alt };
                    }
                    else
                    {
                        estoque.Alteracoes.Add(alt);
                    }
                    using (var db = new AplicativoContext())
                    {
                        db.Estoque.Update(estoque);
                        db.SaveChanges();
                    }

                    var novoValor = estoque.Alteracoes.Sum(x => x.Alteração);
                    serieGrafico.Values.Add(novoValor);
                }
            }
        }
    }
}
