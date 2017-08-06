using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
                var lista = (from est in db.Estoque.ToArray()
                             join prod in db.Produtos.ToArray() on est.Id equals prod.Id
                             orderby prod.Descricao
                             select new Conjunto() { Produto = prod, Estoque = est }).GerarObs();
                cmbProduto.ItemsSource = lista;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
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
            base.OnNavigatingFrom(e);
        }

        private void cmbProduto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var conj = (Conjunto)e.AddedItems[0];
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
            dadosEstoque.DataContext = conj.Estoque;
        }

        struct Conjunto
        {
            public ProdutoDI Produto { get; set; }
            public Estoque Estoque { get; set; }
        }

        private async void AlterarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            var estoque = (Estoque)dadosEstoque.DataContext;
            var caixa = new CaixasDialogo.AlteracaoEstoque();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var valor = caixa.ValorProcessado;
                if (valor != 0)
                {
                    using (var db = new AplicativoContext())
                    {
                        if (estoque.Alteracoes == null)
                        {
                            estoque.Alteracoes = new List<AlteracaoEstoque>();
                        }
                        var alt = new AlteracaoEstoque() { Alteração = valor };
                        estoque.Alteracoes.Add(alt);
                        db.Estoque.Update(estoque);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
