using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.View;
using NFeFacil.View;
using Venda;
using Venda.Impostos;
using Venda.ViewProdutoVenda;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Consumidor
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    [DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class ProdutoNFCe : Page, IValida
    {
        DadosAdicaoProduto Conjunto;
        DetalhesProdutos ProdutoCompleto { get; set; }
        bool PodeConcluir { get; set; }
        public bool Concluido { get; set; }

        public ProdutoNFCe()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Conjunto = (DadosAdicaoProduto)e.Parameter;
            ProdutoCompleto = Conjunto.Completo;
            PodeConcluir = Conjunto.ImpostosPadrao?.Length > 0;
            CalcularTotalBruto();
        }

        double QuantidadeComercializada
        {
            get => ProdutoCompleto.Produto.QuantidadeComercializada;
            set
            {
                ProdutoCompleto.Produto.QuantidadeComercializada = value;
                CalcularTotalBruto();
            }
        }

        double ValorUnitario
        {
            get => ProdutoCompleto.Produto.ValorUnitario;
            set
            {
                ProdutoCompleto.Produto.ValorUnitario = value;
                CalcularTotalBruto();
            }
        }

        void CalcularTotalBruto()
        {
            var valorTotal = QuantidadeComercializada * ValorUnitario;
            ProdutoCompleto.Produto.ValorTotal = valorTotal;
            txtTotalBruto.Text = valorTotal.ToString("C");
        }

        private void Avancar(object sender, RoutedEventArgs e)
        {
            new GerenciadorTributacao(Conjunto).AplicarTributacaoManual();
        }

        void Concluir(object sender, RoutedEventArgs e)
        {
            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter;
            var controle = (IControleViewProdutoFiscal)parametro;
            controle.AplicarTributacaoAutomatica(Conjunto);

            Concluido = true;
            BasicMainPage.Current.Retornar();
        }
    }
}
