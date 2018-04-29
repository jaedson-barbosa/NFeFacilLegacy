using BaseGeral.View;
using NFeFacil.View;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    [DetalhePagina("\uEC59", "Registro de venda")]
    public sealed partial class ManipulacaoProdutosRV : Page, IValida
    {
        ObservableCollection<ExibicaoProdutoListaGeral> Produtos { get; }
        IControleViewProduto Controle { get; set; }

        public bool Concluido => Controle.Concluido;
        Visibility VisibilidadeAvancar { get; set; }
        Visibility VisibilidadeConcluir { get; set; }

        public ManipulacaoProdutosRV()
        {
            InitializeComponent();
            Produtos = new ObservableCollection<ExibicaoProdutoListaGeral>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Controle = (IControleViewProduto)e.Parameter;
            VisibilidadeAvancar = Controle.PodeConcluir ? Visibility.Collapsed : Visibility.Visible;
            VisibilidadeConcluir = Controle.PodeConcluir ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RemoverProduto(object sender, RoutedEventArgs e)
        {
            var prod = ((FrameworkElement)sender).DataContext;
            Controle.Remover((ExibicaoProdutoListaGeral)prod);
        }

        private async void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            var jaAdicionados = Controle.ProdutosAdicionados;
            AdicionarProduto caixa = null;
            caixa = Controle.PodeDetalhar
                ? new AdicionarProduto(jaAdicionados, () => Controle.Adicionar(caixa), Controle.AnalisarDetalhamentoProduto)
                : new AdicionarProduto(jaAdicionados, () => Controle.Adicionar(caixa));
            await caixa.ShowAsync();
            if (caixa.Detalhar) Controle.Detalhar();
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
    }
}
