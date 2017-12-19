using NFeFacil.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarVendedores : Page
    {
        ObservableCollection<ConjuntoBasicoExibicao> Vendedores { get; }

        public GerenciarVendedores()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                var conjuntos = new ObservableCollection<ConjuntoBasicoExibicao>();
                foreach (var atual in repo.ObterVendedores())
                {
                    var novoConjunto = new ConjuntoBasicoExibicao
                    {
                        Objeto = atual,
                        Principal = atual.Item1.Nome,
                        Secundario = ExtensoesPrincipal.AplicarMáscaraDocumento(atual.Item1.CPFStr),
                        Imagem = atual.Item2?.GetSource()
                    };
                    conjuntos.Add(novoConjunto);
                }
                Vendedores = conjuntos.OrderBy(x => x.Principal).GerarObs();
            }
        }

        private void AdicionarVendedor(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarVendedor>();
        }

        private void EditarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var obj = ((ConjuntoBasicoExibicao)contexto).Objeto;
            MainPage.Current.Navegar<AdicionarVendedor>((Vendedor)obj);
        }

        private void InativarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var exib = (ConjuntoBasicoExibicao)contexto;
            var obj = (Vendedor)exib.Objeto;

            using (var repo = new Repositorio.Escrita())
            {
                repo.InativarVendedor(obj, Propriedades.DateTimeNow);
                Vendedores.Remove(exib);
            }
        }

        async void ImagemVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var vend = (ConjuntoBasicoExibicao)contexto;
            var obj = (Vendedor)vend.Objeto;

            var caixa = new View.DefinirImagem(obj.Id, vend.Imagem);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var index = Vendedores.IndexOf(vend);
                vend.Imagem = caixa.Imagem;
                Vendedores[index] = vend;
            }
        }
    }
}
