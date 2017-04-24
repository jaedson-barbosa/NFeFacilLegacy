using BibliotecaCentral.ItensBD;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using BibliotecaCentral.Repositorio;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class NotasSalvas : Page, IEsconde
    {
        public NotasSalvas()
        {
            InitializeComponent();
            using (var db = new NotasFiscais())
            {
                lstNotas.ItemsSource = db.Registro.GerarObs();
            }
            Propriedades.Intercambio.SeAtualizar(Telas.NotasSalvas, Symbol.Library, "Notas salvas");
        }

        private async void RemoverAsync(object sender, RoutedEventArgs e)
        {
            var nota = (sender as FrameworkElement).DataContext as NFeDI;
            using (var db = new NotasFiscais())
            {
                await db.Remover(nota);
                db.SalvarMudancas();
                lstNotas.ItemsSource = db.Registro.GerarObs();
            }
        }

        private async void EditarAsync(object sender, RoutedEventArgs e)
        {
            var nota = (sender as FrameworkElement).DataContext as NFeDI;
            var conjunto = new GrupoViewBanco<(NFeDI, object)>
            {
                ItemBanco = (nota, await nota.ConjuntoCompletoAsync()),
                OperacaoRequirida = TipoOperacao.Edicao
            };
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoNotaFiscal), conjunto);
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
