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
            using (var db = new AplicativoContext())
            {
                var vendedores = db.Vendedores.Where(x => x.Ativo).ToArray();
                var imagens = db.Imagens;
                var quantVendedores = vendedores.Length;
                var conjuntos = new ObservableCollection<ConjuntoBasicoExibicao>();
                for (int i = 0; i < quantVendedores; i++)
                {
                    var atual = vendedores[i];
                    var novoConjunto = new ConjuntoBasicoExibicao
                    {
                        Objeto = atual,
                        Principal = atual.Nome,
                        Secundario = ExtensoesPrincipal.AplicarMáscaraDocumento(atual.CPFStr)
                    };
                    var img = imagens.Find(atual.Id);
                    if (img != null && img.Bytes != null)
                    {
                        var task = img.GetSourceAsync();
                        task.Wait();
                        novoConjunto.Imagem = task.Result;
                    }
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

            using (var db = new AplicativoContext())
            {
                obj.Ativo = false;
                db.Update(obj);
                db.SaveChanges();
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
