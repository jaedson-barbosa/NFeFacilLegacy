using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoRegistroVenda : Page
    {
        RegistroVenda ItemBanco { get; set; } = new RegistroVenda();

        ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; set; }
        ObservableCollection<ClienteDI> Clientes { get; set; }
        ObservableCollection<MotoristaDI> Motoristas { get; set; }

        void AtualizarTotal()
        {
            ItemBanco.DescontoTotal = ItemBanco.Produtos.Sum(x => x.Desconto);

            txtTotal.Text = ItemBanco.Produtos.Sum(x => x.TotalLíquido).ToString("C");
            txtDescontoTotal.Text = ItemBanco.DescontoTotal.ToString("C");
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

        DateTimeOffset DataHoraVenda
        {
            get => ItemBanco.DataHoraVenda;
            set => ItemBanco.DataHoraVenda = value.DateTime;
        }

        public ManipulacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.Where(x => x.Ativo).OrderBy(x => x.Nome).GerarObs();
                Motoristas = db.Motoristas.Where(x => x.Ativo).OrderBy(x => x.Nome).GerarObs();

                MainPage.Current.SeAtualizar("\uEC59", "Registro de venda");
                ItemBanco = new RegistroVenda
                {
                    Emitente = Propriedades.EmitenteAtivo.Id,
                    Vendedor = Propriedades.VendedorAtivo?.Id ?? Guid.Empty,
                    Produtos = new System.Collections.Generic.List<ProdutoSimplesVenda>(),
                    DataHoraVenda = DateTime.Now
                };
                ListaProdutos = new ObservableCollection<ExibicaoProdutoVenda>();
                AtualizarTotal();
            }
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var prod = (ExibicaoProdutoVenda)((FrameworkElement)sender).DataContext;
            ListaProdutos.Remove(prod);
            ItemBanco.Produtos.Remove(prod.Base);
        }

        private async void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarProduto();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoProdBanco = new ProdutoSimplesVenda
                {
                    IdBase = caixa.ProdutoSelecionado.Base.Id,
                    ValorUnitario = caixa.ProdutoSelecionado.PreçoDouble,
                    Quantidade = caixa.Quantidade,
                    Frete = caixa.Frete,
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
            using (var db = new AplicativoContext())
            {
                var log = Popup.Current;
                ItemBanco.UltimaData = DateTime.Now;
                db.Add(ItemBanco);
                ItemBanco.Produtos.ForEach(x => x.RegistrarAlteracaoEstoque(db));
                log.Escrever(TitulosComuns.Sucesso, "Registro de venda salvo com sucesso.");
                db.SaveChanges();
            }

            var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
            PageStackEntry entrada = new PageStackEntry(typeof(VisualizacaoRegistroVenda), ItemBanco, new Windows.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo());
            Frame.BackStack.Add(entrada);

            MainPage.Current.Retornar(true);
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
    }

    struct ExibicaoProdutoVenda
    {
        public ProdutoSimplesVenda Base { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public string TotalLíquido => Base.TotalLíquido.ToString("C");
    }
}
