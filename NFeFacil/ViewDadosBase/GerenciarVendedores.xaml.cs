using BaseGeral;
using BaseGeral.ItensBD;
using NFeFacil.View;
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
        ConjuntoBasicoExibicao<Vendedor>[] TodosVendedores { get; }
        ObservableCollection<ConjuntoBasicoExibicao<Vendedor>> Vendedores { get; }

        public GerenciarVendedores()
        {
            InitializeComponent();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosVendedores = repo.ObterVendedores().Select(atual => new ConjuntoBasicoExibicao<Vendedor>
                {
                    Objeto = atual.Item1,
                    Principal = atual.Item1.Nome,
                    Secundario = ExtensoesPrincipal.AplicarMáscaraDocumento(atual.Item1.CPFStr),
                    Imagem = atual.Item2?.GetSource()
                }).OrderBy(x => x.Principal).ToArray();
                Vendedores = TodosVendedores.GerarObs();
            }
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
                Vendedores.Remove(exib);
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
                var index = Vendedores.IndexOf(vend);
                vend.Imagem = caixa.Imagem;
                Vendedores[index] = vend;
            }
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosVendedores.Length; i++)
            {
                var atual = TodosVendedores[i];
                bool valido = DefinicoesPermanentes.ModoBuscaVendedor == 0
                    ? atual.Principal.ToUpper().Contains(busca.ToUpper())
                    : atual.Objeto.CPFStr.Contains(busca);
                if (valido && !Vendedores.Contains(atual))
                {
                    Vendedores.Add(atual);
                }
                else if (!valido && Vendedores.Contains(atual))
                {
                    Vendedores.Remove(atual);
                }
            }
        }
    }
}
