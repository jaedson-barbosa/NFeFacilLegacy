using NFeFacil.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarProdutos : Page
    {
        ObservableCollection<ProdutoDI> Produtos { get; }

        public GerenciarProdutos()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                Produtos = db.Produtos.Where(x => x.Ativo).OrderBy(x => x.Descricao).GerarObs();
            }
        }

        private void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarProduto>();
        }

        private void EditarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            MainPage.Current.Navegar<AdicionarProduto>((ProdutoDI)contexto);
        }

        async void ControlarEstoque(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var produto = (ProdutoDI)contexto;
            using (var db = new AplicativoContext())
            {
                if (db.Estoque.Count(x => x.Id == produto.Id) > 0)
                {
                    Log.Popup.Current.Escrever(Log.TitulosComuns.Atenção, "O produto já foi adicionado ao controle de estoque.");
                }
                else
                {
                    var caixa = new MessageDialog("Essa é uma operação sem volta, uma vez adicionado ao controle de estoque este produto será permanentemente parte dele. Certeza que você realmente quer isso?", "Atenção");
                    caixa.Commands.Add(new UICommand("Sim", x =>
                    {
                        db.Estoque.Add(new Estoque() { Id = produto.Id });
                        db.SaveChanges();
                    }));
                    caixa.Commands.Add(new UICommand("Não"));
                    await caixa.ShowAsync();
                }
            }
        }

        private void InativarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var prod = (ProdutoDI)contexto;

            using (var db = new AplicativoContext())
            {
                prod.Ativo = false;
                db.Update(prod);
                db.SaveChanges();
                Produtos.Remove(prod);
            }
        }
    }
}
