using BibliotecaCentral.ItensBD;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML;

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
            var conjunto = new ConjuntoManipuladorNFe
            {
                StatusAtual = (StatusNFe)nota.Status,
                OperacaoRequirida = TipoOperacao.Edicao
            };
            if (nota.Status < 4)
            {
                conjunto.NotaSalva = (await nota.ConjuntoCompletoAsync()) as NFe;
            }
            else
            {
                conjunto.NotaEmitida = (await nota.ConjuntoCompletoAsync()) as Processo;
            }
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoNotaFiscal), conjunto);
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
