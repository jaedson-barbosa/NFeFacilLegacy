using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasEspeciaisProduto
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DefinirVeiculo : Page
    {
        VeiculoNovo Veiculo { get; } = new VeiculoNovo();

        public DefinirVeiculo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar("\uE804", "Veículo");
        }

        private void Concluido(object sender, RoutedEventArgs e)
        {
            var ultFrame = Frame.BackStack[Frame.BackStack.Count - 1];
            var prod = ((DetalhesProdutos)ultFrame.Parameter).Produto;
            prod.veicProd = Veiculo;
            prod.medicamentos = null;
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
