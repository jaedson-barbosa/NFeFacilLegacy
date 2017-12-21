using NFeFacil.ItensBD;
using NFeFacil.View;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    [DetalhePagina(Symbol.Home, "Escolher vendedor")]
    public sealed partial class EscolhaVendedor : Page
    {
        public EscolhaVendedor()
        {
            InitializeComponent();

            using (var repo = new Repositorio.Leitura())
            {
                var conjuntos = new ObservableCollection<ConjuntoBasicoExibicao<Vendedor>>();
                foreach (var atual in repo.ObterVendedores())
                {
                    var novoConjunto = new ConjuntoBasicoExibicao<Vendedor>
                    {
                        Objeto = atual.Item1,
                        Principal = atual.Item1.Nome,
                        Secundario = ExtensoesPrincipal.AplicarMáscaraDocumento(atual.Item1.CPFStr),
                        Imagem = atual.Item2?.GetSource()
                    };
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

        private void VendedorEscolhido(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = (ConjuntoBasicoExibicao<Vendedor>)e.AddedItems[0];
                Propriedades.VendedorAtivo = item.Objeto;
                Propriedades.FotoVendedor = item.Imagem;
                MainPage.Current.Navegar<View.Inicio>();
                MainPage.Current.AtualizarInformaçõesGerais();
            }
        }

        private void LogarAdiministrador(object sender, RoutedEventArgs e)
        {
            Propriedades.VendedorAtivo = null;
            MainPage.Current.Navegar<View.Inicio>();
            MainPage.Current.AtualizarInformaçõesGerais();
        }
    }
}
