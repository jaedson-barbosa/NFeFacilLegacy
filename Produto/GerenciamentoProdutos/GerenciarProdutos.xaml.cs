using BaseGeral;
using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System;
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
        }

        void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Navegar<AdicionarProduto>(new ProdutoDI());
        }

        void EditarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            BasicMainPage.Current.Navegar<AdicionarProduto>((ProdutoDI)contexto);
        }

        async void ControlarEstoque(object sender, RoutedEventArgs e)
        {
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
    }
}
