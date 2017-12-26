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

        ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; set; }
        ObservableCollection<ClienteManipulacaoRV> Clientes { get; set; }
        ObservableCollection<MotoristaDI> Motoristas { get; set; }

        void AtualizarTotal()
        {
            ItemBanco.DescontoTotal = ItemBanco.Produtos.Sum(x => x.Desconto);

            txtTotalBruto.Text = ItemBanco.Produtos.Sum(x => x.Quantidade * x.ValorUnitario).ToString("C");
            txtAdicionais.Text = ItemBanco.Produtos.Sum(x => x.Seguro + x.Frete + x.DespesasExtras).ToString("C");
            txtDescontoTotal.Text = ItemBanco.DescontoTotal.ToString("C");
            txtTotal.Text = ItemBanco.Produtos.Sum(x => x.TotalLíquido).ToString("C");
        }

        ClienteManipulacaoRV Cliente
        {
            get => Clientes.FirstOrDefault(x => x.Root.Id == ItemBanco.Cliente);
            set
            {
                ItemBanco.Cliente = value.Root.Id;
                if (!string.IsNullOrEmpty(value.Root.CNPJ))
                {
                    DefinirComprador(value);
                }
            }
        }

        async void DefinirComprador(ClienteManipulacaoRV client)
        {
            var compradores = client.Compradores;
            var nomes = compradores.Select(x => x.Nome);
            var caixa = new DefinirComprador(nomes);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var escolhido = compradores.First(x => x.Nome == caixa.Escolhido);
                ItemBanco.Comprador = escolhido.Id;
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
            }
            ItemBanco = new RegistroVenda
            {
                Emitente = DefinicoesTemporarias.EmitenteAtivo.Id,
                Vendedor = DefinicoesTemporarias.VendedorAtivo?.Id ?? Guid.Empty,
                Produtos = new System.Collections.Generic.List<ProdutoSimplesVenda>(),
                DataHoraVenda = DefinicoesTemporarias.DateTimeNow,
                PrazoEntrega = DefinicoesTemporarias.DateTimeNow
            };
            ListaProdutos = new ObservableCollection<ExibicaoProdutoVenda>();
            AtualizarTotal();
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var prod = (ExibicaoProdutoVenda)((FrameworkElement)sender).DataContext;
            ListaProdutos.Remove(prod);
            ItemBanco.Produtos.Remove(prod.Base);
        }

        private async void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarProduto(ListaProdutos.Select(x => x.Base.IdBase).ToArray());
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoProdBanco = new ProdutoSimplesVenda
                {
                    IdBase = caixa.ProdutoSelecionado.Base.Id,
                    ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble,
                    Quantidade = caixa.Quantidade,
                    Frete = 0,
                    Seguro = caixa.Seguro,
                    DespesasExtras = caixa.DespesasExtras
                };
                novoProdBanco.CalcularTotalLíquido();
                var novoProdExib = new ExibicaoProdutoVenda
                {
                    Base = novoProdBanco,
                    Descricao = caixa.ProdutoSelecionado.Nome,
                    Quantidade = novoProdBanco.Quantidade,
                };
                ListaProdutos.Add(novoProdExib);
                ItemBanco.Produtos.Add(novoProdBanco);
                AtualizarTotal();
            }
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
                    var antigo = ListaProdutos[i];
                    antigo.Base = atual;
                    ListaProdutos[i] = antigo;
                    ItemBanco.Produtos[i] = antigo.Base;
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
                for (int i = 0; i < ListaProdutos.Count; i++)
                {
                    var atual = ListaProdutos[i];
                    atual.Base.Frete = caixa.Valor / ListaProdutos.Count;
                    atual.Base.CalcularTotalLíquido();
                    ListaProdutos[i] = atual;
                    ItemBanco.Produtos[i] = atual.Base;
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
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public string TotalBruto => (Base.ValorUnitario * Quantidade + Base.Seguro + Base.DespesasExtras).ToString("C");
    }

    struct ClienteManipulacaoRV
    {
        public ClienteDI Root { get; set; }
        public Comprador[] Compradores { get; set; }
    }
}
