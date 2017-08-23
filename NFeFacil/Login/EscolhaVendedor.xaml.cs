using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class EscolhaVendedor : Page
    {
        public EscolhaVendedor()
        {
            this.InitializeComponent();

            using (var db = new AplicativoContext())
            {
                var vendedores = db.Vendedores.ToArray();
                var imagens = db.Imagens;
                var quantVendedores = vendedores.Length;
                var conjuntos = new ObservableCollection<ConjuntoBasicoExibicaoVendedor>();
                for (int i = 0; i < quantVendedores; i++)
                {
                    var atual = vendedores[i];
                    var novoConjunto = new ConjuntoBasicoExibicaoVendedor
                    {
                        IdVendedor = atual.Id,
                        Nome = atual.Nome
                    };
                    var img = imagens.Find(atual.Id);
                    if (img != null)
                    {
                        var task = img.GetSourceAsync();
                        task.Wait();
                        novoConjunto.Imagem = task.Result;
                    }
                    conjuntos.Add(novoConjunto);
                }
                grdVendedores.ItemsSource = conjuntos;
            }
        }

        private async void VendedorEscolhido(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = (ConjuntoBasicoExibicaoVendedor)e.AddedItems[0];
                using (var db = new AplicativoContext())
                {
                    var vend = db.Vendedores.Find(item.IdVendedor);
                    Propriedades.VendedorAtivo = vend;
                }
                MainPage.Current.Navegar<View.Inicio>();
                await MainPage.Current.AtualizarInformaçõesGerais();
            }
        }

        private async void LogarAdiministrador(object sender, RoutedEventArgs e)
        {
            Propriedades.VendedorAtivo = null;
            MainPage.Current.Navegar<View.Inicio>();
            await MainPage.Current.AtualizarInformaçõesGerais();
        }

        struct ConjuntoBasicoExibicaoVendedor
        {
            public Guid IdVendedor { get; set; }
            public ImageSource Imagem { get; set; }
            public string Nome { get; set; }
        }
    }
}
