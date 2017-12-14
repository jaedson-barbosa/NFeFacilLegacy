using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
                grdVendedores.ItemsSource = conjuntos;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame.BackStack.Clear();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        private async void VendedorEscolhido(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = (ConjuntoBasicoExibicao)e.AddedItems[0];
                Propriedades.VendedorAtivo = (Vendedor)item.Objeto;
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
    }
}
