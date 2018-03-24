using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using BaseGeral.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.ProdutoEspecial
{
    [DetalhePagina(DetalhePagina.SimbolosEspeciais.Arma, "Armamento")]
    public sealed partial class DefinirArmamentos : Page
    {
        ObservableCollection<Arma> Armas { get; set; }

        public DefinirArmamentos()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var prod = e.Parameter as IProdutoEspecial;
            Armas = prod?.armas?.GerarObs() ?? new ObservableCollection<Arma>();
        }

        async void AdicionarArmamento(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarArmamento();
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
            var prod = (IProdutoEspecial)ultFrame.Parameter;
            prod.veicProd = null;
            prod.medicamentos = null;
            prod.armas = new List<Arma>(Armas);
            prod.comb = null;
            prod.NRECOPI = null;

            BasicMainPage.Current.Retornar();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Retornar();
        }
    }
}
