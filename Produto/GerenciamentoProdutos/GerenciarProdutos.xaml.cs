using BaseGeral;
using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using BaseGeral.Log;
using BaseGeral.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    [DetalhePagina(Symbol.Manage, "Gerenciar produtos")]
    public sealed partial class GerenciarProdutos : Page
    {
        BuscadorProduto Produtos { get; }
        
        public GerenciarProdutos()
        {
            InitializeComponent();
            Produtos = new BuscadorProduto();
            IniciarSistemaClassificacao();
        }

        void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Navegar<AdicionarProduto>(new ProdutoDI());
        }

        void EditarProduto(object sender, RoutedEventArgs e)
        {
            if (VerificarClassificacaoEmUso()) return;
            var contexto = ((FrameworkElement)sender).DataContext;
            BasicMainPage.Current.Navegar<AdicionarProduto>((ProdutoDI)contexto);
        }

        async void ControlarEstoque(object sender, RoutedEventArgs e)
        {
            if (VerificarClassificacaoEmUso()) return;
            var contexto = ((FrameworkElement)sender).DataContext;
            var produto = (ProdutoDI)contexto;
            Estoque estoque = null;
            using (var leit = new BaseGeral.Repositorio.Leitura())
            {
                estoque = leit.ObterEstoque(produto.Id);
            }
            if (estoque == null)
            {
                using (var repo = new BaseGeral.Repositorio.Escrita())
                {
                    var caixa = new MessageDialog("Essa é uma operação sem volta, uma vez adicionado ao controle de estoque este produto será permanentemente parte dele. Certeza que você realmente quer isso?", "Atenção");
                    caixa.Commands.Add(new UICommand("Sim", x =>
                    {
                        estoque = new Estoque() { Id = produto.Id };
                        repo.SalvarItemSimples(estoque, DefinicoesTemporarias.DateTimeNow);
                        repo.SalvarComTotalCerteza();
                    }));
                    caixa.Commands.Add(new UICommand("Não"));
                    if ((await caixa.ShowAsync()).Label == "Não") return;
                }
                BasicMainPage.Current.Navegar<ControleEstoque>(estoque);
            }
            else
            {
                BasicMainPage.Current.Navegar<ControleEstoque>(estoque);
            }
        }

        void InativarProduto(object sender, RoutedEventArgs e)
        {
            if (VerificarClassificacaoEmUso()) return;
            var contexto = ((FrameworkElement)sender).DataContext;
            var prod = (ProdutoDI)contexto;

            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(prod, DefinicoesTemporarias.DateTimeNow);
                Produtos.Remover(prod);
            }
        }

        void Buscar(object sender, TextChangedEventArgs e)
        {
            string busca = ((TextBox)sender).Text;
            Produtos.Buscar(busca);
        }

        ObservableCollection<FornecedorDI> Fornecedores { get; set; }
        ObservableCollection<CategoriaDI> Categorias { get; set; }
        Visibility ClassificavelF { get; set; }
        Visibility ClassificavelC { get; set; }
        bool ClassificacaoEmUso;
        TipoClassificacao Current;
        Guid IdFornecedor, IdCategoria;

        void IniciarSistemaClassificacao()
        {
            using (var leitor = new BaseGeral.Repositorio.Leitura())
            {
                Categorias = leitor.ObterCategorias().GerarObs();
                ClassificavelC = Categorias.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                Fornecedores = leitor.ObterFornecedores().GerarObs();
                ClassificavelF = Fornecedores.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        bool VerificarClassificacaoEmUso()
        {
            if (ClassificacaoEmUso)
                Popup.Current.Escrever(TitulosComuns.Atenção, "Não é possível realizar outras operações enquanto estiver sendo feita a classificação dos produtos.");
            return ClassificacaoEmUso;
        }

        void AplicarCategorias(object sender, RoutedEventArgs e)
        {
            Current = TipoClassificacao.Categoria;
            AplicarDesignPreClassificacao();
        }

        void AplicarFornecedores(object sender, RoutedEventArgs e)
        {
            Current = TipoClassificacao.Fornecedor;
            AplicarDesignPreClassificacao();
        }

        void AplicarDesignPreClassificacao()
        {
            ClassificacaoEmUso = true;
            btnAdd.Visibility = Visibility.Collapsed;
            btnConfirmar.Visibility = btnCancelar.Visibility = Visibility.Visible;
            cmbCategoria.Visibility = Current == TipoClassificacao.Categoria
                ? Visibility.Visible : Visibility.Collapsed;
            cmbFornecedor.Visibility = Current == TipoClassificacao.Fornecedor
                ? Visibility.Visible : Visibility.Collapsed;
            Produtos.ValidacaoAdicional = x => Current == TipoClassificacao.Categoria
                ? x.IdCategoria == Guid.Empty
                : x.IdFornecedor == Guid.Empty;
            grdItens.SelectionMode = ListViewSelectionMode.Multiple;
            Produtos.Buscar();
        }

        void AplicarDesignPosClassificacao()
        {
            btnAdd.Visibility = Visibility.Visible;
            btnConfirmar.Visibility = btnCancelar.Visibility = Visibility.Collapsed;
            cmbCategoria.Visibility = cmbFornecedor.Visibility = Visibility.Collapsed;
            Produtos.ValidacaoAdicional = null;
            grdItens.SelectionMode = ListViewSelectionMode.Single;
            Produtos.Buscar();
            ClassificacaoEmUso = false;
        }

        enum TipoClassificacao { Fornecedor, Categoria }

        void ConfirmarClassificacao(object sender, RoutedEventArgs e)
        {
            if (Current == TipoClassificacao.Categoria && IdCategoria == Guid.Empty ||
                Current == TipoClassificacao.Fornecedor && IdFornecedor == Guid.Empty)
            {
                Popup.Current.Escrever(TitulosComuns.Atenção, "Primeiro escolha o valor desejado na caixa de seleção ao lado da caixa de busca.");
                return;
            }
            var itens = grdItens.SelectedItems;
            using (var escrita = new BaseGeral.Repositorio.Escrita())
                for (int i = 0; i < itens.Count; i++)
                {
                    var prod = (ProdutoDI)itens[i];
                    if (Current == TipoClassificacao.Categoria)
                        prod.IdCategoria = IdCategoria;
                    else
                        prod.IdFornecedor = IdFornecedor;
                    escrita.SalvarItemSimples(prod, DefinicoesTemporarias.DateTimeNow);
                }
            AplicarDesignPosClassificacao();
        }

        void CancelarClassificacao(object sender, RoutedEventArgs e)
        {
            AplicarDesignPosClassificacao();
        }
    }
}
