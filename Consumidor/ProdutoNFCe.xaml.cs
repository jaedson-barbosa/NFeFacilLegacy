using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using NFeFacil.Produto;
using NFeFacil.Produto.Impostos;
using NFeFacil.View;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFCe
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
            BasicMainPage.Current.Navegar<EscolhaImpostos>(Conjunto);
        }

        async void Concluir(object sender, RoutedEventArgs e)
        {
            var icms = Conjunto.Auxiliar.GetICMSArmazenados();
            var imps = Conjunto.Auxiliar.GetImpSimplesArmazenados();

            var padrao = Conjunto.ImpostosPadrao;
            IDetalhamentoImposto[] detalhamentos = new IDetalhamentoImposto[padrao.Length];
            for (int i = 0; i < padrao.Length; i++)
            {
                var (Tipo, NomeTemplate, CST) = padrao[i];
                var impPronto = Tipo == PrincipaisImpostos.ICMS ? (ImpostoArmazenado)icms.First(Analisar) : imps.First(Analisar);
                bool Analisar(ImpostoArmazenado x) => x.Tipo == Tipo && x.NomeTemplate == NomeTemplate && x.CST == CST;
                detalhamentos[i] = new DadoPronto { ImpostoPronto = impPronto };
            }
            var roteiro = new RoteiroAdicaoImpostos(detalhamentos, ProdutoCompleto);
            while (roteiro.Avancar()) roteiro.Validar(null);

            var produto = roteiro.Finalizar();
            produto.Impostos.impostos.RemoveAll(x => x.GetType() == typeof(PISST) || x.GetType() == typeof(COFINSST));

            var caixa = new DefinirTotalImpostos();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary && !string.IsNullOrEmpty(caixa.ValorTotalTributos))
            {
                produto.Impostos.vTotTrib = caixa.ValorTotalTributos;
            }
            else
            {
                produto.Impostos.vTotTrib = null;
            }

            var info = ((NFCe)Frame.BackStack[Frame.BackStack.Count - 1].Parameter).Informacoes;
            if (produto.Número == 0)
            {
                produto.Número = info.produtos.Count + 1;
                info.produtos.Add(produto);
            }
            else
            {
                info.produtos[produto.Número - 1] = produto;
            }
            info.total = new Total(info.produtos);

            Concluido = true;
            BasicMainPage.Current.Retornar();
        }
    }
}
