using LiveCharts.Configurations;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase.GerenciamentoProdutos
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ControleEstoque : Page
    {
        bool AlteracoesNaoSalvas = false;
        Estoque Estoque;
        Func<double, string> Formatter { get; set; }

        public string LocalizacaoGenerica
        {
            get => Estoque.LocalizacaoGenerica;
            set
            {
                Estoque.LocalizacaoGenerica = value;
                AlteracoesNaoSalvas = true;
            }
        }

        public string Segmento
        {
            get => Estoque.Segmento;
            set
            {
                Estoque.Segmento = value;
                AlteracoesNaoSalvas = true;
            }
        }

        public string Prateleira
        {
            get => Estoque.Prateleira;
            set
            {
                Estoque.Prateleira = value;
                AlteracoesNaoSalvas = true;
            }
        }

        public string Locacao
        {
            get => Estoque.Locação;
            set
            {
                Estoque.Locação = value;
                AlteracoesNaoSalvas = true;
            }
        }

        public ControleEstoque()
        {
            InitializeComponent();

            paiGrafico.Series.Configuration = Mappers.Xy<DateModel>()
                .X(dayModel => dayModel.IdTempo)
                .Y(dayModel => dayModel.Value);

            Formatter = new Func<double, string>(x => x < 1 || x > 12 ? "Não há dados" : CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)x));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Estoque = (Estoque)e.Parameter;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                db.Update(Estoque);
                db.SaveChanges();
            }
        }

        private void ProdutoAlterado(object sender, SelectionChangedEventArgs e)
        {
            var alteracoes = Estoque.Alteracoes;
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

        public class DateModel
        {
            public int IdTempo { get; set; }
            public double Value { get; set; }
        }

        private async void AlterarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarAlteracaoEstoque();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var valor = caixa.ValorProcessado;
                if (valor != 0)
                {
                    Estoque.UltimaData = Propriedades.DateTimeNow;
                    var alt = new AlteracaoEstoque()
                    {
                        Alteração = valor,
                        MomentoRegistro = Propriedades.DateTimeNow
                    };
                    if (Estoque.Alteracoes == null)
                    {
                        Estoque.Alteracoes = new List<AlteracaoEstoque>() { alt };
                    }
                    else
                    {
                        Estoque.Alteracoes.Add(alt);
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
