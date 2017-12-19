using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
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
                var item = (ConjuntoBasicoExibicao)e.AddedItems[0];
                Propriedades.VendedorAtivo = (Vendedor)item.Objeto;
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
