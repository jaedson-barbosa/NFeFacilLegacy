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
        public RegistroVenda ItemBanco { get; set; } = new RegistroVenda();

        public ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; private set; }
        public ObservableCollection<ClienteDI> Clientes { get; }
        public ObservableCollection<MotoristaDI> Motoristas { get; }

        void AtualizarTotal()
        {
            txtTotal.Text = ItemBanco.Produtos.Sum(x => x.TotalLíquido).ToString("C");
            txtDescontoTotal.Text = ItemBanco.DescontoTotal.ToString("C");
        }

        AplicativoContext db = new AplicativoContext();

        public string Observacoes
        {
            get => ItemBanco.Observações;
            set => ItemBanco.Observações = value;
        }

        public Guid Cliente
        {
            get => ItemBanco.Cliente;
            set => ItemBanco.Cliente = value;
        }

        public Guid Motorista
        {
            get => ItemBanco.Motorista;
            set => ItemBanco.Motorista = value;
        }

        public DateTimeOffset DataHoraVenda
        {
            get => ItemBanco.DataHoraVenda;
            set => ItemBanco.DataHoraVenda = value.DateTime;
        }

        public ManipulacaoRegistroVenda()
        {
            InitializeComponent();

            Clientes = db.Clientes.GerarObs();
            Motoristas = db.Motoristas.GerarObs();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar("\uEC59", "Registro de venda");
            if (e.Parameter == null)
            {
                ItemBanco = new RegistroVenda
                {
                    Emitente = Propriedades.EmitenteAtivo.Id,
                    Cliente = Clientes[0].Id,
                    Produtos = new System.Collections.Generic.List<ProdutoSimplesVenda>(),
                    DataHoraVenda = DateTime.Now
                };
                db.AttachRange(ItemBanco.Produtos);
                ListaProdutos = new ObservableCollection<ExibicaoProdutoVenda>();
            }
            else
            {
                ItemBanco = (RegistroVenda)e.Parameter;
                ListaProdutos = (from prod in ItemBanco.Produtos
                                 select new ExibicaoProdutoVenda
                                 {
                                     Base = prod,
                                     Descricao = db.Produtos.Find(prod.IdBase).Descricao,
                                     Quantidade = prod.Quantidade,
                                 }).GerarObs();
            }
            AtualizarTotal();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            db.Dispose();
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var prod = (ExibicaoProdutoVenda)((FrameworkElement)sender).DataContext;
            if (prod.Base.Id != default(Guid))
            {
                db.Remove(prod.Base);
            }
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
            var log = Popup.Current;
            ItemBanco.UltimaData = DateTime.Now;
            ItemBanco.Vendedor = Propriedades.VendedorAtivo?.Id ?? Guid.Empty;
            if (ItemBanco.Id == Guid.Empty)
            {
                db.Add(ItemBanco);
                ItemBanco.Produtos.ForEach(x => x.RegistrarAlteracaoEstoque(db));
                log.Escrever(TitulosComuns.Sucesso, "Registro de venda salvo com sucesso.");
            }
            else
            {
                var antigo = db.Vendas.Find(ItemBanco.Id);
                antigo.Produtos.ForEach(x => x.DesregistrarAlteracaoEstoque(db));
                ItemBanco.Produtos.ForEach(x => x.RegistrarAlteracaoEstoque(db));
                db.Update(ItemBanco);
                log.Escrever(TitulosComuns.Sucesso, "Registro de venda alterado com sucesso.");
            }
            db.SaveChanges();
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

    public struct ExibicaoProdutoVenda
    {
        public ProdutoSimplesVenda Base { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public string TotalLíquido => Base.TotalLíquido.ToString("C");
    }
}
