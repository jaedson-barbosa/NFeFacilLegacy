using NFeFacil.ModeloXML.PartesProcesso;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Globalization;
using LiveCharts;
using LiveCharts.Uwp;
using LiveCharts.Configurations;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class VendasAnuais : Page
    {
        Func<double, string> GetMonth { get; set; } = x => NomesMeses[(int)x] ?? string.Empty;
        Func<double, string> GetNome { get; set; } = x => NomesClientes?[(int)x] ?? string.Empty;

        SeriesCollection ResultadoMes { get; }
        SeriesCollection ResultadoCliente { get; }

        static string[] NomesMeses = new string[12];
        static string[] NomesClientes = new string[12];

        readonly Dictionary<int, NotaProcessada[]> NotasFiscais;
        readonly ObservableCollection<int> AnosDisponiveis;

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

            using (var db = new AplicativoContext())
            {
                AnosDisponiveis = (from dado in db.NotasFiscais
                                   where dado.CNPJEmitente == Propriedades.EmitenteAtivo.CNPJ
                                   let ano = DateTime.Parse(dado.DataEmissao).Year
                                   orderby ano ascending
                                   select ano).Distinct().GerarObs();
                NotasFiscais = (from item in db.NotasFiscais
                                where item.Status >= 4
                                where item.CNPJEmitente == Propriedades.EmitenteAtivo.CNPJ
                                let data = DateTime.Parse(item.DataEmissao)
                                let xml = XElement.Parse(item.XML)
                                let nota = xml.FirstNode.FromXElement<NFe>()
                                group new NotaProcessada(data, nota) by data.Year).ToDictionary(x => x.Key, x => x.ToArray());
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

        static double Convert(string str) => str?.ToDouble() ?? 0;

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
                foreach (var item in gruposMeses)
                {
                    var atual = new TotalPorMes
                    {
                        Id = i,
                        Quantidade = item.Quant,
                        Total = item.Total
                    };
                    var nomeMes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Mes);
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
            foreach (var item in gruposClientes.Take(11))
            {
                var atual = new TotalPorCliente
                {
                    Id = i,
                    Nome = item.Nome,
                    Quantidade = item.Quant,
                    Total = item.Total
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
                Log.Popup.Current.Escrever(Log.TitulosComuns.Atenção, "Primeiro você deve escolher um ano para ser analisado.");
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
                Total = nota.Informacoes.total.ICMSTot.VNF;
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
