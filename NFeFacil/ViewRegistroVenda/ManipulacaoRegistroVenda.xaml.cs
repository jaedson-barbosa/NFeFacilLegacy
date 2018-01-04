using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    [DetalhePagina("\uEC59", "Registro de venda")]
    public sealed partial class ManipulacaoRegistroVenda : Page, IValida
    {
        RegistroVenda ItemBanco { get; set; } = new RegistroVenda();

        ObservableCollection<ClienteManipulacaoRV> Clientes { get; set; }
        ObservableCollection<MotoristaDI> Motoristas { get; set; }
        ObservableCollection<Comprador> Compradores { get; set; }

        void AtualizarTotal()
        {
            ItemBanco.DescontoTotal = ItemBanco.Produtos.Sum(x => x.Desconto);

            txtTotalAdicionais.Text = ItemBanco.Produtos.Sum(x => x.DespesasExtras).ToString("C");
            txtTotalBruto.Text = ItemBanco.Produtos.Sum(x => x.Quantidade * x.ValorUnitario).ToString("C");
            txtTotalDesconto.Text = ItemBanco.DescontoTotal.ToString("C");
            txtTotalFrete.Text = ItemBanco.Produtos.Sum(x => x.Frete).ToString("C");
            txtTotalLiquido.Text = ItemBanco.Produtos.Sum(x => x.TotalLíquido).ToString("C");
            txtTotalSeguro.Text = ItemBanco.Produtos.Sum(x => x.Seguro).ToString("C");
        }

        ClienteManipulacaoRV Cliente
        {
            get => Clientes.FirstOrDefault(x => x.Root.Id == ItemBanco.Cliente);
            set
            {
                ItemBanco.Cliente = value.Root.Id;
                ItemBanco.Comprador = default(Guid);
                if (value.Compradores.Length > 0)
                {
                    Compradores.Clear();
                    for (int i = 0; i < value.Compradores.Length; i++)
                    {
                        Compradores.Add(Compradores[i]);
                    }
                }
                cmbComprador.IsEnabled = value.Compradores.Length > 0;
            }
        }

        Guid Motorista
        {
            get => ItemBanco.Motorista;
            set => ItemBanco.Motorista = value;
        }

        string Observacoes
        {
            get => ItemBanco.Observações;
            set => ItemBanco.Observações = value;
        }

        DateTimeOffset PrazoEntrega
        {
            get => ItemBanco.PrazoEntrega;
            set => ItemBanco.PrazoEntrega = value.DateTime;
        }

        public bool Concluido { get; private set; }

        public ManipulacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (var repo = new Repositorio.Leitura())
            {
                Clientes = repo.ObterClientesComCompradores().Select(x => new ClienteManipulacaoRV()
                {
                    Root = x.Item1,
                    Compradores = x.Item2
                }).GerarObs();
                Motoristas = repo.ObterMotoristas().GerarObs();
                Compradores = new ObservableCollection<Comprador>();
            }
            ItemBanco = (RegistroVenda)e.Parameter;
            AtualizarTotal();
        }

        private void Finalizar(object sender, RoutedEventArgs e)
        {
            if (ItemBanco.Cliente == default(Guid))
            {
                Popup.Current.Escrever(TitulosComuns.Atenção, "Escolha primeiro um cliente.");
            }
            else
            {
                using (var repo = new Repositorio.Escrita())
                {
                    repo.AdicionarRV(ItemBanco, DefinicoesTemporarias.DateTimeNow);
                }

                var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                PageStackEntry entrada = new PageStackEntry(typeof(VisualizacaoRegistroVenda), ItemBanco, null);
                Frame.BackStack.Add(entrada);

                Concluido = true;
                MainPage.Current.Retornar();
            }
        }

        private async void AplicarDesconto(object sender, RoutedEventArgs e)
        {
            var caixa = new CalculoDesconto(ItemBanco.Produtos);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var prods = caixa.Produtos;
                for (int i = 0; i < prods.Count; i++)
                {
                    var atual = prods[i];
                    atual.CalcularTotalLíquido();
                    ItemBanco.Produtos[i] = atual;
                }
                ItemBanco.DescontoTotal = prods.Sum(x => x.Desconto);
                AtualizarTotal();
            }
        }

        async void AplicarFrete(object sender, RoutedEventArgs e)
        {
            var caixa = new AplicarFrete();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                for (int i = 0; i < ItemBanco.Produtos.Count; i++)
                {
                    var atual = ItemBanco.Produtos[i];
                    atual.Frete = caixa.Valor / ItemBanco.Produtos.Count;
                    atual.CalcularTotalLíquido();
                    ItemBanco.Produtos[i] = atual;
                }
                AtualizarTotal();

                ItemBanco.TipoFrete = caixa.TipoFrete;
            }
        }

        async void DefinirPagamento(object sender, RoutedEventArgs e)
        {
            var caixa = new InfoPagamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ItemBanco.PrazoPagamento = caixa.Prazo.ToString("dd/MM/yyyy");
                ItemBanco.FormaPagamento = caixa.FormaPagamento;
            }
            else
            {
                var input = (AppBarToggleButton)sender;
                input.IsChecked = false;
            }
        }

        void RemoverPagamento(object sender, RoutedEventArgs e)
        {
            ItemBanco.PrazoPagamento = null;
            ItemBanco.FormaPagamento = null;
        }

        async void DefinirDataVenda(object sender, RoutedEventArgs e)
        {
            var caixa = new DefinirDataVenda();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ItemBanco.DataHoraVenda = caixa.Data.DateTime;
            }
        }
    }

    struct ExibicaoProdutoVenda
    {
        public ProdutoSimplesVenda Base { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public string ValorUnitario => Base.ValorUnitario.ToString("C");
        public string TotalBruto => (Base.ValorUnitario * Quantidade + Base.Seguro + Base.DespesasExtras).ToString("C");
    }

    struct ClienteManipulacaoRV
    {
        public ClienteDI Root { get; set; }
        public Comprador[] Compradores { get; set; }
    }
}
