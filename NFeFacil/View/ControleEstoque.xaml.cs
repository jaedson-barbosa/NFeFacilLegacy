using LiveCharts.Configurations;
using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        Func<double, string> Formatter { get; set; }

        public ControleEstoque()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var lista = (from est in db.Estoque.Include(x => x.Alteracoes).ToArray()
                             join prod in db.Produtos.ToArray() on est.Id equals prod.Id
                             orderby prod.Descricao
                             select new Conjunto() { Descricao = prod.Descricao, Estoque = est }).GerarObs();
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Estoque.Alteracoes.OrderBy(x => x.MomentoRegistro);
                }
                cmbProduto.ItemsSource = lista;
                if (lista.Count > 0)
                {
                    cmbProduto.SelectedIndex = 0;
                }
            }

            paiGrafico.Series.Configuration = Mappers.Xy<DateModel>()
                .X(dayModel => dayModel.IdTempo)
                .Y(dayModel => dayModel.Value);

            Formatter = new Func<double, string>(x => x < 1 || x > 12 ? "Não há dados" : CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)x));
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
        }

        private void ProdutoAlterado(object sender, SelectionChangedEventArgs e)
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
                    var anos = new List<Ano>(from item in alteracoes
                                             group item by item.MomentoRegistro.Year into k
                                             select new Ano(k, k.Key));
                    var valores = new DateModel[12];
                    if (anos.Count > 0)
                    {
                        var ano = anos.Last();
                        var anosAnteriores = anos.Take(anos.Count - 1).Sum(x => x.Total);
                        for (int i = 0; i < 12; i++)
                        {
                            var mes = ano.Meses[i];
                            valores[i] = new DateModel()
                            {
                                IdTempo = i + 1,
                                Value = ano.Meses.Take(i).Sum(x => x.Total) + mes.Total + anosAnteriores
                            };
                        }
                        serieGrafico.Values = new LiveCharts.ChartValues<DateModel>(valores);
                    }
                    else
                    {
                        serieGrafico.Values = new LiveCharts.ChartValues<DateModel>(new DateModel[1] { new DateModel() { IdTempo = 0, Value = 0 } });
                    }
                }
                else
                {
                    serieGrafico.Values = new LiveCharts.ChartValues<DateModel>(new DateModel[1] { new DateModel() { IdTempo = 0, Value = 0 } });
                }
            }
        }

        public class DateModel
        {
            public int IdTempo { get; set; }
            public double Value { get; set; }
        }

        struct Conjunto
        {
            public string Descricao { get; set; }
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
                    estoque.UltimaData = Propriedades.DateTimeNow;
                    var alt = new AlteracaoEstoque()
                    {
                        Alteração = valor,
                        MomentoRegistro = Propriedades.DateTimeNow
                    };
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

                    for (int i = alt.MomentoRegistro.Month - 1; i < serieGrafico.Values.Count; i++)
                    {
                        var item = (DateModel)serieGrafico.Values[i];
                        item.Value += valor;
                        serieGrafico.Values.RemoveAt(i);
                        serieGrafico.Values.Insert(i, item);
                    }
                }
            }
        }
    }
}
