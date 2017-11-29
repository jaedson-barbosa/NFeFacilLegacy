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

        readonly Dictionary<int, NFe[]> NotasFiscais;
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

        public VendasAnuais()
        {
            InitializeComponent();

            using (var db = new AplicativoContext())
            {
                AnosDisponiveis = (from dado in db.NotasFiscais
                                   let ano = Convert.ToDateTime(dado.DataEmissao).Year
                                   orderby ano ascending
                                   select ano).Distinct().GerarObs();
                NotasFiscais = (from item in db.NotasFiscais
                                where item.Status >= 4
                                let data = DateTime.Parse(item.DataEmissao)
                                let xml = XElement.Parse(item.XML)
                                let nota = xml.FirstNode.FromXElement<NFe>()
                                group nota by data.Year).ToDictionary(x => x.Key, x => x.ToArray());
            }

            ResultadoMes = new SeriesCollection
            {
                new ColumnSeries()
                {
                    Values = new ChartValues<TotalPorMes>(),
                    Title = "Total",
                    LabelPoint = x => $": {x.Y}",
                    Configuration = Mappers.Xy<TotalPorMes>().X(x => x.Id).Y(x => x.Total)
                },
                new ColumnSeries
                {
                    Values = new ChartValues<TotalPorMes>(),
                    Title = "Quantidade",
                    LabelPoint = x => $": {x.Y}",
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
                    Configuration = Mappers.Xy<TotalPorCliente>().X(x => x.Id).Y(x => x.Total)
                },
                new ColumnSeries
                {
                    Values = new ChartValues<TotalPorCliente>(),
                    Title = "Quantidade",
                    LabelPoint = x => $": {x.Y}",
                    Configuration = Mappers.Xy<TotalPorCliente>().X(x => x.Id).Y(x => x.Quantidade)
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Calendar, "Vendas");
        }

        void AtualizarMeses()
        {
            try
            {
                var gruposMeses = from nota in NotasFiscais[AnoEscolhido]
                                  let data = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão)
                                  group nota by data.Month into item
                                  let total = item.Sum(x => x.Informacoes.total.ICMSTot.VNF)
                                  orderby OrdenacaoMeses == 0 ? -item.Key : total descending
                                  select item;
                ResultadoMes[0].Values.Clear();
                ResultadoMes[1].Values.Clear();
                int i = 0;
                foreach (var item in gruposMeses)
                {
                    var atual = new TotalPorMes
                    {
                        Id = i,
                        Quantidade = item.Sum(det => det.Informacoes.produtos.Sum(prod => prod.Produto.QuantidadeComercializada)),
                        Total = item.Sum(det => det.Informacoes.total.ICMSTot.VNF)
                    };
                    NomesMeses[i++] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key);
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
            var gruposClientes = from nota in NotasFiscais[AnoEscolhido]
                                 group nota by nota.Informacoes.destinatário.Documento into item
                                 let total = item.Sum(x => x.Informacoes.total.ICMSTot.VNF)
                                 let quant = item.Sum(x => x.Informacoes.produtos.Sum(prod => prod.Produto.QuantidadeComercializada))
                                 orderby OrdenacaoClientes == 0 ? total : quant descending
                                 select new { Nome = item.First().Informacoes.destinatário.Nome, Total = total, Quant = quant };
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
