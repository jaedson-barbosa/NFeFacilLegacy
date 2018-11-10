using BaseGeral;
using BaseGeral.Log;
using BaseGeral.View;
using NFeFacil.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    [DetalhePagina("\uEC59", "Produtos")]
    public sealed partial class ListaProdutos : Page, IValida
    {
        ObservableCollection<ExibicaoProdutoListaGeral> Produtos { get; set; }
        IControleViewProduto Controle { get; set; }

        public bool Concluido => Controle.Concluido;
        Visibility VisibilidadeConcluir { get; set; }

        public ListaProdutos()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Controle = (IControleViewProduto)e.Parameter;
            Produtos = Controle.ObterProdutosIniciais();
            Controle.ProdutoAtualizado += Controle_ProdutoAtualizado;
            VisibilidadeConcluir = Controle.PodeConcluir ? Visibility.Visible : Visibility.Collapsed;
            AtualizarTotalLiquido();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Controle.ProdutoAtualizado -= Controle_ProdutoAtualizado;
        }

        private void Controle_ProdutoAtualizado(object sender, (ExibicaoProdutoListaGeral antigo, ExibicaoProdutoListaGeral novo) e)
        {
            int index = Produtos.IndexOf(e.antigo);
            Produtos.RemoveAt(index);
            Produtos.Insert(index, e.novo);
            AtualizarTotalLiquido();
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var context = ((FrameworkElement)sender).DataContext;
            var prod = (ExibicaoProdutoListaGeral)context;
            Produtos.Remove(prod);
            Controle.Remover(prod);
            AtualizarTotalLiquido();
        }

        private void Editar(object sender, RoutedEventArgs e)
        {
            var log = Popup.Current;
            var context = ((FrameworkElement)sender).DataContext;
            var prod = (ExibicaoProdutoListaGeral)context;
            Controle.Editar(prod);
        }

        private async void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            var jaAdicionados = Controle.ProdutosAdicionados;
            AdicionarProduto caixa = null;
            caixa = Controle.PodeDetalhar
                ? new AdicionarProduto(jaAdicionados, Adicionar, Controle.AnalisarDetalhamento, Controle.AnalMudançaValorUnit)
                : new AdicionarProduto(jaAdicionados, Adicionar);
            await caixa.ShowAsync();
            if (caixa.Detalhar) Controle.Detalhar(caixa);

            void Adicionar()
            {
                var prod = Controle.Adicionar(caixa);
                Produtos.Add(prod);
                AtualizarTotalLiquido();
            }
        }

        void AtualizarTotalLiquido()
        {
            txtTotalLiquido.Text = Produtos.Sum(x => x.TotalLiquidoD).ToString("C");
        }

        void Avancar(object sender, RoutedEventArgs e)
        {
            if (Controle.Validar())
                Controle.Avancar();
        }

        void Concluir(object sender, RoutedEventArgs e)
        {
            if (Controle.Validar())
                Controle.Concluir();
        }

        void Ordenar(object sender, RoutedEventArgs e)
        {
            popOrdenar.IsOpen = !popOrdenar.IsOpen;
        }

        void OrdenarPorDescricao(object sender, RoutedEventArgs e)
        {
            Produtos.Sort(x => x.Descricao, true);
            popOrdenar.IsOpen = false;
        }

        void OrdenarPorCodigo(object sender, RoutedEventArgs e)
        {
            Produtos.Sort(x => x.Codigo, true);
            popOrdenar.IsOpen = false;
        }
    }
}
