using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.ProdutoEspecial
{
    [View.DetalhePagina("\uE804", "Veículo")]
    public sealed partial class DefinirVeiculo : Page
    {
        VeiculoNovo Veiculo { get; set; }

        public DefinirVeiculo()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var prod = e.Parameter as IProdutoEspecial;
            Veiculo = prod?.veicProd ?? new VeiculoNovo();
        }

        private void Concluido(object sender, RoutedEventArgs e)
        {
            var ultFrame = Frame.BackStack[Frame.BackStack.Count - 1];
            var prod = (IProdutoEspecial)ultFrame.Parameter;
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
