using BaseGeral.ModeloXML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using System.Globalization;
using LiveCharts;
using LiveCharts.Uwp;
using LiveCharts.Configurations;
using static BaseGeral.ExtensoesPrincipal;
using Windows.UI.Xaml.Navigation;
using BaseGeral;
using BaseGeral.Log;
using BaseGeral.View;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    [DetalhePagina(Symbol.Calendar, "Vendas")]
    public sealed partial class VendasAnuais : Page
    {
        Func<double, string> GetMonth { get; set; } = x => NomesMeses[(int)x] ?? string.Empty;
        Func<double, string> GetNome { get; set; } = x => NomesClientes?[(int)x] ?? string.Empty;

        SeriesCollection ResultadoMes { get; set; }
        SeriesCollection ResultadoCliente { get; set; }

        static string[] NomesMeses = new string[12];
        static string[] NomesClientes = new string[12];

        Dictionary<int, NotaProcessada[]> NotasFiscais;
        ObservableCollection<int> AnosDisponiveis;

        int anoEscolhido;
        int AnoEscolhido
        {
            get => anoEscolhido;
            set
            {
                anoEscolhido = value;
                AtualizarMeses();
                AtualizarClientes();
            }
        }

        int ordenacaoMeses;
        int OrdenacaoMeses
        {
            get => ordenacaoMeses;
            set
            {
                ordenacaoMeses = value;
                AtualizarMeses();
            }
        }

        int ordenacaoClientes;
        int OrdenacaoClientes
        {
            get => ordenacaoClientes;
            set
            {
                ordenacaoClientes = value;
                AtualizarClientes();
            }
        }

        IEnumerable<string> ProdutosFiltrados;
        int TipoTotal;

        public VendasAnuais()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var isNFCe = e.Parameter != null ? (bool)e.Parameter : false;
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                AnosDisponiveis = repo.ObterAnosNotas(DefinicoesTemporarias.EmitenteAtivo.CNPJ, isNFCe).GerarObs();
                NotasFiscais = repo.ObterNFesPorAno(DefinicoesTemporarias.EmitenteAtivo.CNPJ, isNFCe)
                    .ToDictionary(x => x.Key, x => x.Value.Select(Processar).ToArray());

                NotaProcessada Processar((DateTime data, string xml) k)
                {
                    var noNFe = XElement.Parse(k.xml).FirstNode;
                    return new NotaProcessada(k.data, noNFe.FromXElement<NFe>());
                }
            }

            ResultadoMes = new SeriesCollection
            {
                new ColumnSeries()
                {
                    Values = new ChartValues<TotalPorMes>(),
                    Title = "Total",
                    LabelPoint = x => $": {x.Y}",
                    MaxColumnWidth = 20,
                    Configuration = Mappers.Xy<TotalPorMes>().X(x => x.Id).Y(x => x.Total)
                },
                new ColumnSeries
                {
                    Values = new ChartValues<TotalPorMes>(),
                    Title = "Quantidade",
                    LabelPoint = x => $": {x.Y}",
                    MaxColumnWidth = 20,
                    Configuration = Mappers.Xy<TotalPorMes>().X(x => x.Id).Y(x => x.Quantidade)
                }
            };

            ResultadoCliente = new SeriesCollection
            {
                new ColumnSeries()
                {
                    Values = new ChartValues<TotalPorCliente>(),
                    Title = "Total",
                    LabelPoint = x => $": {x.Y}",
                    MaxColumnWidth = 20,
                    Configuration = Mappers.Xy<TotalPorCliente>().X(x => x.Id).Y(x => x.Total)
                },
                new ColumnSeries
                {
                    Values = new ChartValues<TotalPorCliente>(),
                    Title = "Quantidade",
                    LabelPoint = x => $": {x.Y}",
                    MaxColumnWidth = 20,
                    Configuration = Mappers.Xy<TotalPorCliente>().X(x => x.Id).Y(x => x.Quantidade)
                }
            };
        }

        static double Convert(string str) => string.IsNullOrEmpty(str) ? 0 : Parse(str);

        void AtualizarMeses()
        {
            try
            {
                IEnumerable<(double Quant, double Total, int Mes)> gruposMeses;
                if (ProdutosFiltrados == null)
                {
                    gruposMeses = from nota in NotasFiscais[AnoEscolhido]
                                  group nota by nota.Mes into item
                                  let total = item.Sum(x => x.Total)
                                  let quant = item.Sum(x => x.Produtos.Sum(prod => prod.Quantidade))
                                  orderby OrdenacaoMeses == 0 ? -item.Key : total descending
                                  select (quant, total, item.Key);
                }
                else
                {
                    gruposMeses = from nota in NotasFiscais[AnoEscolhido]
                                  group nota by nota.Mes into item
                                  let produtos = item.SelectMany(x => x.Produtos.Where(prod => ProdutosFiltrados.Contains(prod.Descricao)))
                                  let total = produtos.Sum(prod => TipoTotal == 0 ? prod.TotalBruto : prod.TotalLiquido)
                                  let quant = produtos.Sum(prod => prod.Quantidade)
                                  orderby OrdenacaoMeses == 0 ? -item.Key : total descending
                                  select (quant, total, item.Key);
                }
                ResultadoMes[0].Values.Clear();
                ResultadoMes[1].Values.Clear();
                int i = 0;
                foreach (var (Quant, Total, Mes) in gruposMeses)
                {
                    var atual = new TotalPorMes
                    {
                        Id = i,
                        Quantidade = Quant,
                        Total = Total
                    };
                    var nomeMes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes);
                    string primeiraLetra = nomeMes[0].ToString();
                    nomeMes = nomeMes.Remove(0, 1).Insert(0, primeiraLetra.ToUpper());
                    NomesMeses[i++] = nomeMes;
                    ResultadoMes[0].Values.Add(atual);
                    ResultadoMes[1].Values.Add(atual);
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        void AtualizarClientes()
        {
            IEnumerable<(string Nome, double Quant, double Total)> gruposClientes;
            if (ProdutosFiltrados == null)
            {
                gruposClientes = from nota in NotasFiscais[AnoEscolhido]
                                 group nota by nota.DocumentoCliente into item
                                 let total = item.Sum(x => x.Total)
                                 let quant = item.Sum(x => x.Produtos.Sum(prod => prod.Quantidade))
                                 orderby OrdenacaoClientes == 0 ? total : quant descending
                                 select (item.First().NomeCliente, quant, total);
            }
            else
            {
                gruposClientes = from nota in NotasFiscais[AnoEscolhido]
                                 group nota by nota.DocumentoCliente into item
                                 let produtos = item.SelectMany(x => x.Produtos.Where(prod => ProdutosFiltrados.Contains(prod.Descricao)))
                                 let total = produtos.Sum(prod => TipoTotal == 0 ? prod.TotalBruto : prod.TotalLiquido)
                                 let quant = produtos.Sum(prod => prod.Quantidade)
                                 orderby OrdenacaoClientes == 0 ? total : quant descending
                                 select (item.First().NomeCliente, quant, total);
            }
            ResultadoCliente[0].Values.Clear();
            ResultadoCliente[1].Values.Clear();
            int i = 0;
            foreach (var (Nome, Quant, Total) in gruposClientes.Take(11))
            {
                var atual = new TotalPorCliente
                {
                    Id = i,
                    Nome = Nome,
                    Quantidade = Quant,
                    Total = Total
                };
                NomesClientes[i++] = atual.Nome;
                ResultadoCliente[0].Values.Add(atual);
                ResultadoCliente[1].Values.Add(atual);
            }

            var itensRestantes = gruposClientes.Skip(11);
            var restante = new TotalPorCliente
            {
                Id = i,
                Nome = "Restante",
                Quantidade = itensRestantes.Sum(x => x.Quant),
                Total = itensRestantes.Sum(x => x.Total)
            };
            NomesClientes[i++] = restante.Nome;
            ResultadoCliente[0].Values.Add(restante);
            ResultadoCliente[1].Values.Add(restante);
        }

        async void FiltrarProdutosAnalisados(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (AnoEscolhido != 0)
            {
                var prods = NotasFiscais[AnoEscolhido].SelectMany(x => x.Produtos.Select(k => k.Descricao));
                var caixa = new EscolhaProdutos(prods.Distinct());
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    ProdutosFiltrados = caixa.Escolhidos;
                    TipoTotal = caixa.TipoTotal;
                    AtualizarMeses();
                    AtualizarClientes();
                }
            }
            else
            {
                ((AppBarToggleButton)sender).IsChecked = false;
                Popup.Current.Escrever(TitulosComuns.Atenção, "Primeiro você deve escolher um ano para ser analisado.");
            }
        }

        private void RemoverFiltro(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ProdutosFiltrados != null)
            {
                ProdutosFiltrados = null;
                AtualizarMeses();
                AtualizarClientes();
            }
        }

        struct NotaProcessada
        {
            public int Mes { get; set; }
            public string NomeCliente { get; set; }
            public string DocumentoCliente { get; set; }
            public double Total { get; set; }
            public IEnumerable<ProdutoProcessado> Produtos { get; set; }

            public NotaProcessada(DateTime data, NFe nota)
            {
                Mes = data.Month;
                NomeCliente = nota.Informacoes.destinatário.Nome;
                DocumentoCliente = nota.Informacoes.destinatário.Documento;
                Total = nota.Informacoes.total.ICMSTot.vNF;
                Produtos = nota.Informacoes.produtos.Select(x => new ProdutoProcessado
                {
                    Descricao = x.Produto.Descricao,
                    Quantidade = x.Produto.QuantidadeComercializada,
                    TotalBruto = x.Produto.ValorTotal,
                    TotalLiquido = x.Produto.ValorTotal + Convert(x.Produto.Frete) + Convert(x.Produto.DespesasAcessorias) + Convert(x.Produto.Seguro) - Convert(x.Produto.Desconto)
                });
            }
        }

        struct ProdutoProcessado
        {
            public string Descricao { get; set; }
            public double Quantidade { get; set; }
            public double TotalBruto { get; set; }
            public double TotalLiquido { get; set; }
        }

        struct TotalPorCliente
        {
            public int Id { get; set; }
            public double Quantidade { get; set; }
            public double Total { get; set; }
            public string Nome { get; set; }
        }

        struct TotalPorMes
        {
            public int Id { get; set; }
            public double Quantidade { get; set; }
            public double Total { get; set; }
        }
    }
}
