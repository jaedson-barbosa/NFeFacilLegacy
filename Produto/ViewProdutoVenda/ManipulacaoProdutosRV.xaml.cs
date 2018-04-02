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
    [DetalhePagina("\uEC59", "Registro de venda")]
    public sealed partial class ManipulacaoProdutosRV : Page, IValida
    {
        IControleViewProduto Controle { get; set; }

        public bool Concluido => Controle.Concluido;
        Visibility VisibilidadeAvancar { get; set; }
        Visibility VisibilidadeConcluir { get; set; }

        public ManipulacaoProdutosRV()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Controle = (IControleViewProduto)e.Parameter;
            VisibilidadeAvancar = Controle.PodeConcluir ? Visibility.Collapsed : Visibility.Visible;
            VisibilidadeConcluir = Controle.PodeConcluir ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var prod = (ExibicaoProdutoVenda)((FrameworkElement)sender).DataContext;
            Controle.Produtos.Remove(prod);
        }

        private async void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            AdicionarProduto caixa = null;
            caixa = new AdicionarProduto(Controle.Produtos.Select(x => x.IdBase).ToArray(), () =>
            {
                var novoProdExib = new ExibicaoProdutoVenda
                {
                    IdBase = caixa.ProdutoSelecionado.Base.Id,
                    ValorUnitario = caixa.ProdutoSelecionado.PrecoDouble,
                    Codigo = caixa.ProdutoSelecionado.Codigo,
                    Descricao = caixa.ProdutoSelecionado.Nome,
                    Quantidade = caixa.Quantidade,
                    Frete = 0,
                    Seguro = caixa.Seguro,
                    DespesasExtras = caixa.DespesasExtras,
                    Desconto = 0
                };
                Controle.Produtos.Add(novoProdExib);
            }, Controle.PodeDetalhar);
            await caixa.ShowAsync();
            if (caixa.Detalhar) Controle.Detalhar();
        }

        void Avancar(object sender, RoutedEventArgs e) => Controle.Avancar();
        void Concluir(object sender, RoutedEventArgs e) => Controle.Concluir();
    }

    public interface IControleViewProduto
    {
        bool Concluido { get; }
        bool PodeConcluir { get; }
        bool PodeDetalhar { get; }
        ObservableCollection<ExibicaoProdutoVenda> Produtos { get; }
        void Avancar();
        void Concluir();
        void Detalhar();
    }
}
