using LiveCharts;
using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase.GerenciamentoProdutos
{
    [View.DetalhePagina(Symbol.Manage, "Controle de estoque")]
    public sealed partial class ControleEstoque : Page
    {
        bool AlteracoesNaoSalvas = false;
        Estoque Estoque;
        ChartValues<double> Valores { get; } = new ChartValues<double>();
        string[] Labels { get; } = new string[10];

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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Estoque = (Estoque)e.Parameter;

            var alteracoes = Estoque.Alteracoes;
            if (alteracoes != null)
            {
                var quant = alteracoes.Count;
                if (quant > 10)
                {
                    var totInicial = alteracoes.Take(quant - 10).Sum(x => x.Alteração);
                    Valores[0] = totInicial;
                    Labels[0] = "Inicial";
                    for (int i = 1; i < 10; i++)
                    {
                        var atual = alteracoes[quant - 10 + i];
                        Valores.Add(atual.Alteração + Valores[i - 1]);
                        Labels[i] = atual.MomentoRegistro.ToString("dd/MM/yyyy");
                    }
                }
                else if (quant > 0)
                {
                    var salto = 10 - quant;
                    for (int i = 0; i < salto; i++)
                    {
                        Valores.Add(alteracoes[0].Alteração);
                        Labels[i] = "Inicial";
                    }
                    for (int i = salto, j = 0; i < 10; i++, j++)
                    {
                        var atual = alteracoes[j];
                        Valores.Add(atual.Alteração + Valores[i - 1]);
                        Labels[i] = atual.MomentoRegistro.ToString("dd/MM/yyyy");
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++) Valores.Add(0);
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (AlteracoesNaoSalvas)
            {
                using (var repo = new Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(Estoque, DefinicoesTemporarias.DateTimeNow);
                }
            }
        }

        private async void AlterarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarAlteracaoEstoque();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var valor = caixa.ValorProcessado;
                if (valor != 0)
                {
                    Estoque.UltimaData = DefinicoesTemporarias.DateTimeNow;
                    var alt = new AlteracaoEstoque()
                    {
                        Alteração = valor,
                        MomentoRegistro = DefinicoesTemporarias.DateTimeNow
                    };
                    if (Estoque.Alteracoes == null)
                    {
                        Estoque.Alteracoes = new List<AlteracaoEstoque>() { alt };
                    }
                    else
                    {
                        Estoque.Alteracoes.Add(alt);
                    }

                    var novosValores = new double[10];
                    for (int i = 0; i < 9; i++)
                    {
                        novosValores[i] = Valores[i + 1];
                        Labels[i] = Labels[i + 1];
                    }
                    novosValores[9] = novosValores[8] + valor;
                    Labels[9] = alt.MomentoRegistro.ToString("dd/MM/yyyy");

                    Valores.Clear();
                    Valores.AddRange(novosValores);

                    AlteracoesNaoSalvas = true;
                }
            }
        }
    }
}
