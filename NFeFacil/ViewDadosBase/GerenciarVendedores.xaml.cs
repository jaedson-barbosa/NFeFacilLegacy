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
        ObservableCollection<ExibicaoVendedor> Vendedores { get; }

        public GerenciarVendedores()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var vendedores = db.Vendedores.Where(x => x.Ativo).ToArray();
                var imagens = db.Imagens;
                var quantVendedores = vendedores.Length;
                var conjuntos = new ObservableCollection<ExibicaoVendedor>();
                for (int i = 0; i < quantVendedores; i++)
                {
                    var atual = vendedores[i];
                    var novoConjunto = new ExibicaoVendedor
                    {
                        Id = atual.Id,
                        Principal = atual.Nome,
                        Secundario = ExtensoesPrincipal.AplicarMáscaraDocumento(atual.CPFStr),
                        Vendedor = atual
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
            MainPage.Current.Navegar<AdicionarVendedor>(((ExibicaoVendedor)contexto).Vendedor);
        }

        private void InativarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var exib = (ExibicaoVendedor)contexto;

            var vend = exib.Vendedor;
            using (var db = new AplicativoContext())
            {
                vend.Ativo = false;
                db.Update(vend);
                db.SaveChanges();
                Vendedores.Remove(exib);
            }
        }

        async void ImagemVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var vend = (ExibicaoVendedor)contexto;
            var caixa = new View.DefinirImagem(vend.Id, vend.Imagem);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var index = Vendedores.IndexOf(vend);
                vend.Imagem = caixa.Imagem;
                Vendedores[index] = vend;
            }
        }
    }

    sealed class ExibicaoVendedor : ConjuntoBasicoExibicao
    {
        public Vendedor Vendedor { get; set; }
        public string Endereco => Vendedor.Endereço;
    }
}
