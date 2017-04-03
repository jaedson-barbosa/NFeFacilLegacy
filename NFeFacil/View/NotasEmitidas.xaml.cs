using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class NotasEmitidas : Page
    {
        public NotasEmitidas()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                lstNotas.ItemsSource = db.NotasFiscais.GerarObs();
            }
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
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoNotaFiscal), await nota.ConjuntoCompletoAsync());
        }
    }
}
