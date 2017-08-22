using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GeralEmitente : Page
    {
        EmitenteDI emitente;

        public GeralEmitente()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var emitente = (ConjuntoBasicoExibicaoEmitente)e.Parameter;
            imgLogotipo.Source = emitente.Imagem;
            txtNome.Text = emitente.Nome;
            using (var db = new AplicativoContext())
            {
                Propriedades.EmitenteAtivo = db.Emitentes.Find(emitente.IdEmitente);
            }
        }

        private void Confirmar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Propriedades.EmitenteAtivo = emitente;
            MainPage.Current.Navegar<EscolhaVendedor>();
        }

        private void Editar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarEmitente>(new GrupoViewBanco<EmitenteDI>()
            {
                ItemBanco = emitente,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private void Apagar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                db.Remove(emitente);
                db.SaveChanges();
            }
            MainPage.Current.Retornar();
        }
    }
}
