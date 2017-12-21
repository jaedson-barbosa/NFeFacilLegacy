using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.ProdutoEspecial
{
    [View.DetalhePagina("\uE95E", "Medicamentos")]
    public sealed partial class DefinirMedicamentos : Page
    {
        ObservableCollection<Medicamento> Medicamentos { get; } = new ObservableCollection<Medicamento>();

        public DefinirMedicamentos()
        {
            InitializeComponent();
        }

        async void AdicionarMedicamento(object sender, RoutedEventArgs e)
        {
            var caixa = new CaixasDialogoProduto.AdicionarMedicamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoMedicamento = caixa.Contexto;
                Medicamentos.Add(novoMedicamento);
            }
        }

        void RemoverMedicamento(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            Medicamentos.Remove((Medicamento)contexto);
        }

        private void Concluido(object sender, RoutedEventArgs e)
        {
            var ultFrame = Frame.BackStack[Frame.BackStack.Count - 1];
            var prod = ((DetalhesProdutos)ultFrame.Parameter).Produto;
            prod.veicProd = null;
            prod.medicamentos = new List<Medicamento>(Medicamentos);
            prod.armas = null;
            prod.comb = null;
            prod.NRECOPI = null;

            MainPage.Current.Retornar();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
