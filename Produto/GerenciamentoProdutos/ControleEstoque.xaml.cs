using LiveCharts;
using BaseGeral.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BaseGeral;
using BaseGeral.View;
using Windows.UI.Popups;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    [DetalhePagina(Symbol.Manage, "Controle de estoque")]
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
                    Valores.Add(totInicial);
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
                        Valores.Add(0);
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
                    for (int i = 0; i < 10; i++)
                    {
                        Valores.Add(0);
                        Labels[i] = "Inicial";
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    Valores.Add(0);
                    Labels[i] = "Inicial";
                }
            }
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (AlteracoesNaoSalvas)
                using (var repo = new BaseGeral.Repositorio.Escrita())
                    repo.SalvarItemSimples(Estoque, DefinicoesTemporarias.DateTimeNow);
        }

        private async void AlterarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarAlteracaoEstoque();
            var resultAdd = await caixa.ShowAsync() == ContentDialogResult.Primary;
            if (caixa.Valor != 0)
            {
                var valorProcessado = caixa.Valor * (resultAdd ? 1 : -1);
                Estoque.UltimaData = DefinicoesTemporarias.DateTimeNow;
                var alt = new AlteracaoEstoque()
                {
                    Alteração = valorProcessado,
                    MomentoRegistro = DefinicoesTemporarias.DateTimeNow
                };
                if (Estoque.Alteracoes == null)
                    Estoque.Alteracoes = new List<AlteracaoEstoque>() { alt };
                else
                    Estoque.Alteracoes.Add(alt);

                var novosValores = new double[10];
                for (int i = 0; i < 9; i++)
                {
                    novosValores[i] = Valores[i + 1];
                    Labels[i] = Labels[i + 1];
                }
                novosValores[9] = novosValores[8] + valorProcessado;
                Labels[9] = alt.MomentoRegistro.ToString("dd/MM/yyyy");

                Valores.Clear();
                Valores.AddRange(novosValores);

                AlteracoesNaoSalvas = true;
            }
        }

        async void RemoverControle_Click(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var msg = new MessageDialog("Você tem certeza disso?\n" +
                "A única diferença é que não durante a adição dos produtos na venda não será analisado o estoque, porém ele não será realmente removido.\n" +
                "A operação poderá ser desfeita sem perda de informação caso se arrependa.", "Certeza?");
            msg.Commands.Add(new UICommand("Sim"));
            msg.Commands.Add(new UICommand("Não", x =>
            {
                Estoque.IsAtivo = false;
                AlteracoesNaoSalvas = true;
            }));
            await msg.ShowAsync();
        }
    }
}
