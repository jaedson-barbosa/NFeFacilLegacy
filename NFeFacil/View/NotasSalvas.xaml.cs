using NFeFacil.ItensBD;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

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
            using (var db = new AplicativoContext())
            {
                lstNotas.ItemsSource = db.NotasFiscais.GerarObs();
            }
            Propriedades.Intercambio.SeAtualizar(Telas.NotasSalvas, Symbol.Library, "Notas salvas");
        }

        private async void RemoverAsync(object sender, RoutedEventArgs e)
        {
            var nota = (sender as FrameworkElement).DataContext as NFeDI;
            await new PastaNotasFiscais().Remover(nota.Id);
            using (var db = new AplicativoContext())
            {
                db.Remove(nota);
                await db.SaveChangesAsync();
                lstNotas.ItemsSource = db.NotasFiscais.GerarObs();
            }
        }

        private async void EditarAsync(object sender, RoutedEventArgs e)
        {
            var nota = (sender as FrameworkElement).DataContext as NFeDI;
            var conjunto = await nota.ConjuntoCompletoAsync();
            conjunto.tipoRequisitado = TipoOperacao.Edicao;
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoNotaFiscal), conjunto);
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
