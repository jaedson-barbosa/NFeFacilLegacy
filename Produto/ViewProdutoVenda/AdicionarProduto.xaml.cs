using BaseGeral;
using BaseGeral.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public sealed partial class AdicionarProduto : ContentDialog, INotifyPropertyChanged
    {
        BuscadorProduto Produtos { get; }
        public ExibicaoProdutoAdicao ProdutoSelecionado { get; set; }

        public double Quantidade { get; set; }
        public double Seguro { get; set; }
        public double DespesasExtras { get; set; }

        bool PodeEspecificarValorUnitario { get; set; } = true;
        public double ValorUnitario { get; set; }

        bool PodeDetalhar { get; set; } = false;
        public bool Detalhar
        {
            get => PrimaryButtonText == "Detalhar";
            private set => PrimaryButtonText = value ? "Detalhar" : "Adicionar";
        }

        Action Adicionar { get; }
        Func<ProdutoAdicao, bool> AnalisarDetalhamento { get; }
        Func<ProdutoAdicao, bool> AnalisarVlUnitario { get; }

        public AdicionarProduto(Dictionary<Guid, double> produtosJaAdicionados, Action adicionar)
        {
            InitializeComponent();
            Adicionar = adicionar;
            Produtos = new BuscadorProduto(produtosJaAdicionados);
        }

        public AdicionarProduto(Dictionary<Guid, double> produtosJaAdicionados, Action adicionar,
            Func<ProdutoAdicao, bool> analisarDetalhamento, Func<ProdutoAdicao, bool> analisarVlUnitario)
            : this(produtosJaAdicionados, adicionar)
        {
            PodeDetalhar = true;
            AnalisarDetalhamento = analisarDetalhamento;
            AnalisarVlUnitario = analisarVlUnitario;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Produtos.Buscar(busca);
        }

        void BotaoPrimario_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                args.Cancel = true;
                var log = Popup.Current;
                if (ProdutoSelecionado?.Base == null)
                    log.Escrever(TitulosComuns.Atenção, "Escolha um produto.");
                else if (Quantidade <= 0)
                    log.Escrever(TitulosComuns.Atenção, "Insira uma quantidade maior que 0.");
                else if (ProdutoSelecionado.EstoqueDouble != double.PositiveInfinity && Quantidade > ProdutoSelecionado.EstoqueDouble)
                    log.Escrever(TitulosComuns.Atenção, "A quantidade vendida não pode ser maior que a quantidade em estoque.");
                else if (Detalhar)
                    args.Cancel = false;
                else
                {
                    Adicionar?.Invoke();
                    if (DefinicoesPermanentes.IgnorarProdutosJaAdicionados)
                        Produtos.Remover(ProdutoSelecionado);
                    else
                    {
                        var novoEstoque = ProdutoSelecionado.EstoqueDouble - Quantidade;
                        if (novoEstoque == 0)
                        {
                            Produtos.Remover(ProdutoSelecionado);
                        }
                        else
                        {
                            ProdutoSelecionado.EstoqueDouble = novoEstoque;
                            ProdutoSelecionado.AplicarAlteracoes();
                            Produtos.AplicarAlteracaoEstoque(ProdutoSelecionado, novoEstoque);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                try
                {
                    e.ManipularErro();
                }
                catch (Exception) { }
            }
        }

        void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (PodeDetalhar)
            {
                var origin = (ListView)sender;
                origin.SelectionChanged += NovoProdutoEscolhido;
            }
        }

        void NovoProdutoEscolhido(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            var exibicaoProduto = (ExibicaoProdutoAdicao)e.AddedItems[0];
            var produto = (ProdutoAdicao)exibicaoProduto;
            var necessitaDetalhamento = AnalisarDetalhamento(produto);
            if (necessitaDetalhamento)
            {
                Detalhar = true;
                PodeDetalhar = false;
            }
            else
            {
                Detalhar = false;
                PodeDetalhar = true;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Detalhar)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PodeDetalhar)));

            PodeEspecificarValorUnitario = AnalisarVlUnitario(produto);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PodeEspecificarValorUnitario)));
        }

        void AnalisarVlNovoProduto(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            var exibicaoProduto = (ExibicaoProdutoAdicao)e.AddedItems[0];
            var produto = (ProdutoAdicao)exibicaoProduto;

            ValorUnitario = produto.Preco;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValorUnitario)));
        }

        sealed class BuscadorProduto : BaseGeral.Buscador.BaseBuscador<ExibicaoProdutoAdicao>
        {
            public BuscadorProduto(Dictionary<Guid, double> produtosJaAdicionados) : base(DefinicoesPermanentes.ModoBuscaProduto)
            {
                using (var repo = new BaseGeral.Repositorio.Leitura())
                {
                    var ListaCompletaProdutos = new List<ProdutoAdicao>();
                    var bloquearRepeticao = DefinicoesPermanentes.IgnorarProdutosJaAdicionados;
                    var estoque = repo.ObterEstoques().ToArray();
                    foreach (var item in repo.ObterProdutos().ToArray())
                    {
                        var jaAdicionado = produtosJaAdicionados.TryGetValue(item.Id, out double quantAdicionada);
                        if (bloquearRepeticao && jaAdicionado) continue;
                        var est = estoque.FirstOrDefault(x => x.Id == item.Id);
                        double quant = est != null ? est.Alteracoes.Sum(x => x.Alteração) : double.PositiveInfinity,
                            quantRestante = quant - (jaAdicionado ? quantAdicionada : 0);
                        if (quantRestante > 0)
                        {
                            var novoProd = new ProdutoAdicao
                            {
                                Base = item,
                                Codigo = item.CodigoProduto,
                                Nome = item.Descricao,
                                Estoque = quantRestante,
                                Preco = item.ValorUnitario
                            };
                            ListaCompletaProdutos.Add(novoProd);
                        }
                    }
                    ListaCompletaProdutos.Sort((a, b) => a.Nome.CompareTo(b.Nome));
                    if (ListaCompletaProdutos.Count == 0)
                    {
                        Popup.Current.Escrever(TitulosComuns.Atenção, "Não existem mais produtos adicionáveis.");
                    }
                    TodosItens = ListaCompletaProdutos.Select(x => (ExibicaoProdutoAdicao)x).ToArray();
                    Itens = TodosItens.GerarObs();
                }
            }

            protected override (string, string) ItemComparado(ExibicaoProdutoAdicao item, int modoBusca)
            {
                switch (modoBusca)
                {
                    case 0: return (item.Nome, null);
                    case 1: return (item.Codigo, null);
                    default: return (item.Nome, item.Codigo);
                }
            }

            protected override void InvalidarItem(ExibicaoProdutoAdicao item, int modoBusca)
            {
                switch (modoBusca)
                {
                    case 0:
                        item.Nome = InvalidProduct;
                        break;
                    case 1:
                        item.Codigo = InvalidProduct;
                        break;
                    default:
                        item.Nome = item.Codigo = InvalidProduct;
                        break;
                }
            }

            public void AplicarAlteracaoEstoque(ExibicaoProdutoAdicao exibicaoProduto, double novoEstoque)
            {
                TodosItens.First(x => x.Base == exibicaoProduto.Base).EstoqueDouble = novoEstoque;
            }
        }
    }
}
