using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasEspeciaisProduto
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DefinirMedicamentos : Page
    {
        public DefinirMedicamentos()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<Medicamento> Medicamentos { get; } = new ObservableCollection<Medicamento>();

        async void AdicionarMedicamento(object sender, RoutedEventArgs e)
        {
            var caixa = new CaixasDialogoProduto.AdicionarMedicamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoMedicamento = (Medicamento)caixa.DataContext;
                Medicamentos.Add(novoMedicamento);
            }
        }

        void RemoverMedicamento(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            Medicamentos.Remove((Medicamento)contexto);
        }
    }
}
