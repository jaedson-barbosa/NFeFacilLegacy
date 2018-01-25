using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.Produto;
using NFeFacil.Produto.Impostos;
using NFeFacil.View;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFCe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ProdutoNFCe : Page, IValida
    {
        DadosAdicaoProduto Conjunto;
        public DetalhesProdutos ProdutoCompleto { get; private set; }
        bool PodeConcluir { get; set; }
        public bool Concluido { get; set; }

        public ProdutoNFCe()
        {
            InitializeComponent();
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
            MainPage.Current.Navegar<EscolhaImpostos>(Conjunto);
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
            var caixa = new DefinirTotalImpostos();
            await caixa.ShowAsync();
            if (!string.IsNullOrEmpty(caixa.ValorTotalTributos))
            {
                produto.Impostos.vTotTrib = caixa.ValorTotalTributos;
            }
            else
            {
                produto.Impostos.vTotTrib = null;
            }

            var info = ((NFe)Frame.BackStack[Frame.BackStack.Count - 1].Parameter).Informacoes;
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
            MainPage.Current.Retornar();
        }
    }
}
