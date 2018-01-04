using NFeFacil.ItensBD;
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
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoProdutosRV : Page, IValida
    {
        RegistroVenda ItemBanco;
        ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; set; }

        public bool Concluido => false;

        public ManipulacaoProdutosRV()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ItemBanco = (RegistroVenda)e.Parameter;
            using (var leitura = new Repositorio.Leitura())
            {
                var prods = from prod in ItemBanco.Produtos
                            let comp = leitura.ObterProduto(prod.IdBase)
                            select new ExibicaoProdutoVenda
                            {
                                Base = prod,
                                Quantidade = prod.Quantidade,
                                Codigo = comp.CodigoProduto,
                                Descricao = comp.Descricao
                            };
                ListaProdutos = prods.GerarObs();
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
            AdicionarProduto caixa = null;
            caixa = new AdicionarProduto(ListaProdutos.Select(x => x.Base.IdBase).ToArray(), () =>
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
                    Codigo = caixa.ProdutoSelecionado.Codigo,
                    Descricao = caixa.ProdutoSelecionado.Nome,
                    Quantidade = novoProdBanco.Quantidade
                };
                ListaProdutos.Add(novoProdExib);
                ItemBanco.Produtos.Add(novoProdBanco);
            });
            await caixa.ShowAsync();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<ManipulacaoRegistroVenda>();
        }
    }
}
