using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public sealed partial class AdicionarProduto : ContentDialog
    {
        List<ExibicaoProdutoAdicao> ListaCompletaProdutos { get; }
        public ObservableCollection<ExibicaoProdutoAdicao> Produtos { get; }
        public ExibicaoProdutoAdicao ProdutoSelecionado { get; set; }

        bool PodeDetalhar { get; }
        public double Quantidade { get; set; }
        public double Seguro { get; set; }
        public double DespesasExtras { get; set; }
        public bool Detalhar { get; private set; }

        Action Adicionar { get; }

        public AdicionarProduto(Guid[] produtosJaAdicionados, Action adicionar, bool podeDetalhar)
        {
            InitializeComponent();
            Adicionar = adicionar;
            PodeDetalhar = podeDetalhar;

            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                ListaCompletaProdutos = new List<ExibicaoProdutoAdicao>();
                var estoque = repo.ObterEstoques();
                foreach (var item in repo.ObterProdutos())
                {
                    if (!produtosJaAdicionados.Contains(item.Id))
                    {
                        var est = estoque.FirstOrDefault(x => x.Id == item.Id);
                        var quant = est != null ? est.Alteracoes.Sum(x => x.Alteração) : 0;
                        if (est == null || quant > 0)
                        {
                            var novoProd = new ExibicaoProdutoAdicao
                            {
                                Base = item,
                                Codigo = item.CodigoProduto,
                                Nome = item.Descricao,
                                Estoque = est == null ? "Infinito" : quant.ToString("N"),
                                Preco = item.ValorUnitario.ToString("C"),
                                PrecoDouble = item.ValorUnitario
                            };
                            ListaCompletaProdutos.Add(novoProd);
                        }
                    }
                }
                ListaCompletaProdutos.Sort((a, b) => a.Nome.CompareTo(b.Nome));
                if (ListaCompletaProdutos.Count == 0)
                {
                    Popup.Current.Escrever(TitulosComuns.Atenção, "Não existem mais produtos adicionáveis.");
                }
                Produtos = ListaCompletaProdutos.GerarObs();
            }
        }

        void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < ListaCompletaProdutos.Count; i++)
            {
                var atual = ListaCompletaProdutos[i];
                bool valido;
                if (DefinicoesPermanentes.ModoBuscaProduto == 0)
                {
                    valido = atual.Nome.ToUpper().Contains(busca.ToUpper());
                }
                else
                {
                    valido = atual.Codigo.ToUpper().Contains(busca.ToUpper());
                }
                if (valido && !Produtos.Contains(atual))
                {
                    Produtos.Add(atual);
                }
                else if (!valido && Produtos.Contains(atual))
                {
                    Produtos.Remove(atual);
                }
            }
        }

        void VerificarConformidadeEstoque(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = !Detalhar;
            var log = Popup.Current;
            if (ProdutoSelecionado?.Base == null)
            {
                log.Escrever(TitulosComuns.Atenção, "Escolha um produto.");
            }
            else if (Quantidade <= 0)
            {
                log.Escrever(TitulosComuns.Atenção, "Insira uma quantidade maior que 0.");
            }
            else if (ProdutoSelecionado.Estoque != "Infinito" && Quantidade > double.Parse(ProdutoSelecionado.Estoque))
            {
                log.Escrever(TitulosComuns.Atenção, "A quantidade vendida não pode ser maior que a quantidade em estoque.");
            }
            else
            {
                Adicionar?.Invoke();
                ListaCompletaProdutos.Remove(ProdutoSelecionado);
                Produtos.Remove(ProdutoSelecionado);
            }
        }
    }

    public sealed class ExibicaoProdutoAdicao
    {
        public ProdutoDI Base { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Estoque { get; set; }
        public double PrecoDouble { get; set; }
        public string Preco { get; set; }
    }
}
