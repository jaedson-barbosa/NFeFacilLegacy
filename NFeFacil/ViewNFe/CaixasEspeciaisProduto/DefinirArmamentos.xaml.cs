using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasEspeciaisProduto
{
    [DetalhePagina(DetalhePagina.SimbolosEspeciais.Arma, "Armamento")]
    public sealed partial class DefinirArmamentos : Page
    {
        ObservableCollection<Arma> Armas { get; } = new ObservableCollection<Arma>();

        public DefinirArmamentos()
        {
            InitializeComponent();
        }

        async void AdicionarArmamento(object sender, RoutedEventArgs e)
        {
            var caixa = new CaixasDialogoProduto.AdicionarArmamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoArmamento = caixa.Contexto;
                Armas.Add(novoArmamento);
            }
        }

        void RemoverArmamento(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            Armas.Remove((Arma)contexto);
        }

        private void Concluido(object sender, RoutedEventArgs e)
        {
            var ultFrame = Frame.BackStack[Frame.BackStack.Count - 1];
            var prod = ((DetalhesProdutos)ultFrame.Parameter).Produto;
            prod.veicProd = null;
            prod.medicamentos = null;
            prod.armas = new List<Arma>(Armas);
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
