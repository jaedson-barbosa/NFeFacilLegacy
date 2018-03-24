using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.GerenciamentoProdutos
{
    [DetalhePagina(Symbol.Manage, "Gerenciar produtos")]
    public sealed partial class GerenciarProdutos : Page
    {
        ProdutoDI[] TodosProdutos { get; }
        ObservableCollection<ProdutoDI> Produtos { get; }

        public GerenciarProdutos()
        {
            InitializeComponent();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosProdutos = repo.ObterProdutos().ToArray();
                Produtos = TodosProdutos.GerarObs();
            }
        }

        private void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Navegar<AdicionarProduto>(new ProdutoDI());
        }

        private void EditarProduto(object sender, RoutedEventArgs e)
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

        private void InativarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var prod = (ProdutoDI)contexto;

            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(prod, DefinicoesTemporarias.DateTimeNow);
                Produtos.Remove(prod);
            }
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosProdutos.Length; i++)
            {
                var atual = TodosProdutos[i];
                bool valido = (DefinicoesPermanentes.ModoBuscaVendedor == 0
                    ? atual.Descricao : atual.CodigoProduto).ToUpper().Contains(busca.ToUpper());
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
    }
}
