using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina("\uF168", "Gerenciar categorias")]
    public sealed partial class GerenciarCategorias : Page
    {
        ObservableCollection<CategoriaDI> Categorias { get; }

        public GerenciarCategorias()
        {
            InitializeComponent();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                Categorias = repo.ObterCategorias().GerarObs();
            }
        }

        async void AdicionarCategoria(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarCategoria();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var newCategoria = new CategoriaDI() { Nome = caixa.Nome };
                using (var repo = new BaseGeral.Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(newCategoria, DefinicoesTemporarias.DateTimeNow);
                }
                Categorias.Add(newCategoria);
            }
        }

        async void EditarCategoria(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var categoria = (CategoriaDI)contexto;
            var index = Categorias.IndexOf(categoria);
            var caixa = new AdicionarCategoria(categoria.Nome);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                categoria.Nome = caixa.Nome;
                using (var repo = new BaseGeral.Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(categoria, DefinicoesTemporarias.DateTimeNow);
                }
                Categorias.RemoveAt(index);
                Categorias.Insert(index, categoria);
            }
        }
    }
}
