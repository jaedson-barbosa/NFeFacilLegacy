using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.View;
using NFeFacil.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum
{
    [DetalhePagina("\uEC59", "Registro de venda")]
    public sealed partial class ManipulacaoProdutosRV : Page, IValida
    {
        RegistroVenda ItemBanco;
        ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; set; }

        public bool Concluido { get; set; }
        Visibility VisibilidadeAvancar { get; set; }
        Visibility VisibilidadeConcluir { get; set; }

        public ManipulacaoProdutosRV()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ItemBanco = (RegistroVenda)e.Parameter;
            var novo = string.IsNullOrEmpty(ItemBanco.MotivoEdicao);
            VisibilidadeAvancar = novo ? Visibility.Visible : Visibility.Collapsed;
            VisibilidadeConcluir = novo ? Visibility.Collapsed : Visibility.Visible;
            using (var leitura = new BaseGeral.Repositorio.Leitura())
            {
                ListaProdutos = (from prod in ItemBanco.Produtos
                                 let comp = leitura.ObterProduto(prod.IdBase)
                                 select new ExibicaoProdutoVenda
                                 {
                                     Base = prod,
                                     Quantidade = prod.Quantidade,
                                     Codigo = comp.CodigoProduto,
                                     Descricao = comp.Descricao
                                 }).GerarObs();
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

        private void Avancar(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Navegar<ManipulacaoRegistroVenda>();
        }

        private void Concluir(object sender, RoutedEventArgs e)
        {
            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.SalvarRV(ItemBanco, DefinicoesTemporarias.DateTimeNow);
                Concluido = true;
                BasicMainPage.Current.Retornar();
            }
        }
    }
}
