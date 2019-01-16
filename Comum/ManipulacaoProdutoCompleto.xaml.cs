using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.Controles;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using NFeFacil.View;
using Venda.Impostos;
using Venda;
using BaseGeral.View;
using BaseGeral;
using Venda.ViewProdutoVenda;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum
{
    [DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class ManipulacaoProdutoCompleto : Page, IHambuguer, IValida
    {
        DadosAdicaoProduto Conjunto;
        public DetalhesProdutos ProdutoCompleto { get; private set; }

        public ImpostoDevol ContextoImpostoDevol
        {
            get
            {
                if (ProdutoCompleto.ImpostoDevol == null)
                {
                    ProdutoCompleto.ImpostoDevol = new ImpostoDevol();
                }
                return ProdutoCompleto.ImpostoDevol;
            }
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

        public ManipulacaoProdutoCompleto()
        {
            InitializeComponent();
        }

        void CalcularTotalBruto()
        {
            var valorTotal = QuantidadeComercializada * ValorUnitario;
            ProdutoCompleto.Produto.ValorTotal = valorTotal;
            txtTotalBruto.Text = valorTotal.ToString("C");
        }

        bool PodeConcluir { get; set; }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var produto = (DadosAdicaoProduto)e.Parameter;
            Conjunto = produto;
            ProdutoCompleto = produto.Completo;
            PodeConcluir = produto.ImpostosPadrao?.Length > 0;
            CalcularTotalBruto();
        }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Tag, "Dados"),
            new ItemHambuguer("\uE825", "Imposto devolvido"),
            new ItemHambuguer(Symbol.Comment, "Info adicional")
        };

        public int SelectedIndex { set => main.SelectedIndex = value; }

        public bool Concluido { get; private set; } = false;

        private void Avancar(object sender, RoutedEventArgs e)
        {
            var porcentDevolv = ProdutoCompleto.ImpostoDevol.pDevol;
            if (string.IsNullOrEmpty(porcentDevolv) || int.Parse(porcentDevolv) == 0)
            {
                ProdutoCompleto.ImpostoDevol = null;
            }
            new GerenciadorTributacao(Conjunto).AplicarTributacaoManual();
        }

        void Concluir(object sender, RoutedEventArgs e)
        {
            var porcentDevolv = ProdutoCompleto.ImpostoDevol.pDevol;
            if (string.IsNullOrEmpty(porcentDevolv) || int.Parse(porcentDevolv) == 0)
            {
                ProdutoCompleto.ImpostoDevol = null;
            }

            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter;
            var controle = (IControleViewProdutoFiscal)parametro;
            controle.AplicarTributacaoAutomatica(Conjunto);

            Concluido = true;
            BasicMainPage.Current.Retornar();
        }
    }
}
