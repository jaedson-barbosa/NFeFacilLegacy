using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using BaseGeral.ModeloXML.PartesDetalhes;
using System;
using BaseGeral.Controles;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using System.Collections.Generic;
using NFeFacil.View;
using Venda.Impostos;
using System.Linq;
using BaseGeral.ModeloXML;
using Venda;
using Venda.CaixasDialogoProduto;
using BaseGeral.View;
using BaseGeral;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum
{
    [DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class ManipulacaoProdutoCompleto : Page, IHambuguer, IValida
    {
        DadosAdicaoProduto Conjunto;
        public DetalhesProdutos ProdutoCompleto { get; private set; }

        public ObservableCollection<DeclaracaoImportacao> ListaDI { get; } = new ObservableCollection<DeclaracaoImportacao>();
        public ObservableCollection<GrupoExportacao> ListaGE { get; } = new ObservableCollection<GrupoExportacao>();

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
            new ItemHambuguer(Symbol.Comment, "Info adicional"),
            new ItemHambuguer(Symbol.World, "Importação"),
            new ItemHambuguer(Symbol.World, "Exportação"),
        };

        public int SelectedIndex { set => main.SelectedIndex = value; }

        public bool Concluido { get; private set; } = false;

        private void Avancar(object sender, RoutedEventArgs e)
        {
            ProdutoCompleto.Produto.DI = new List<DeclaracaoImportacao>(ListaDI);
            ProdutoCompleto.Produto.GrupoExportação = new List<GrupoExportacao>(ListaGE);

            var porcentDevolv = ProdutoCompleto.ImpostoDevol.pDevol;
            if (string.IsNullOrEmpty(porcentDevolv) || int.Parse(porcentDevolv) == 0)
            {
                ProdutoCompleto.ImpostoDevol = null;
            }
            BasicMainPage.Current.Navegar<EscolhaImpostos>(Conjunto);
        }

        async void AdicionarDeclaracaoImportacao(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarDeclaracaoImportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ListaDI.Add(caixa.Declaracao);
            }
        }

        void RemoverDeclaracaoImportacao(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            ListaDI.Remove((DeclaracaoImportacao)contexto);
        }

        async void AdicionarDeclaracaoExportacao(object sender, RoutedEventArgs e)
        {
            var caixa = new EscolherTipoDeclaracaoExportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                if (caixa.Direta)
                {
                    var caixa2 = new AddDeclaracaoExportacaoDireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ListaGE.Add(caixa2.Declaracao);
                    }
                }
                else
                {
                    var caixa2 = new AddDeclaracaoExportacaoIndireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ListaGE.Add(caixa2.Declaracao);
                    }
                }
            }
        }

        void RemoverDeclaracaoExportacao(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            ListaGE.Remove((GrupoExportacao)contexto);
        }

        async void Concluir(object sender, RoutedEventArgs e)
        {
            ProdutoCompleto.Produto.DI = new List<DeclaracaoImportacao>(ListaDI);
            ProdutoCompleto.Produto.GrupoExportação = new List<GrupoExportacao>(ListaGE);

            var porcentDevolv = ProdutoCompleto.ImpostoDevol.pDevol;
            if (string.IsNullOrEmpty(porcentDevolv) || int.Parse(porcentDevolv) == 0)
            {
                ProdutoCompleto.ImpostoDevol = null;
            }

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
            if (await caixa.ShowAsync() == ContentDialogResult.Primary && !string.IsNullOrEmpty(caixa.ValorTotalTributos))
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
            BasicMainPage.Current.Retornar();
        }
    }
}
