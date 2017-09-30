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
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var emitente = (ConjuntoBasicoExibicaoEmitente)e.Parameter;
            imgLogotipo.Source = emitente.Imagem;
            txtNomeFantasia.Text = emitente.NomeFantasia;
            txtNome.Text = emitente.Nome;
            using (var db = new AplicativoContext())
            {
                this.emitente = db.Emitentes.Find(emitente.IdEmitente);
            }
            MainPage.Current.SeAtualizar(Symbol.Home, "Dados da empresa");
        }

        private void Confirmar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Propriedades.EmitenteAtivo = emitente;
            MainPage.Current.Navegar<EscolhaVendedor>();
        }

        private void Editar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarEmitente>(emitente);
        }
    }
}
