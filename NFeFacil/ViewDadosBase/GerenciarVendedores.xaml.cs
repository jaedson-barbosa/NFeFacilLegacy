using BaseGeral;
using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.Manage, "Gerenciar vendedores")]
    public sealed partial class GerenciarVendedores : Page
    {
        BuscadorVendedor Vendedores { get; }

        public GerenciarVendedores()
        {
            InitializeComponent();
            Vendedores = new BuscadorVendedor();
        }

        private void AdicionarVendedor(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarVendedor>();
        }

        private void EditarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var obj = ((ConjuntoBasicoExibicao<Vendedor>)contexto).Objeto;
            MainPage.Current.Navegar<AdicionarVendedor>(obj);
        }

        private void InativarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var exib = (ConjuntoBasicoExibicao<Vendedor>)contexto;
            var obj = exib.Objeto;

            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(obj, DefinicoesTemporarias.DateTimeNow);
                Vendedores.Remover(exib);
            }
        }

        async void ImagemVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var vend = (ConjuntoBasicoExibicao<Vendedor>)contexto;
            var obj = vend.Objeto;

            var caixa = new View.DefinirImagem(obj.Id, vend.Imagem);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Vendedores.AtualizarImagem(caixa.Imagem, vend);
            }
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Vendedores.Buscar(busca);
        }
    }
}
