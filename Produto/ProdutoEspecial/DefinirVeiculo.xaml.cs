using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using BaseGeral.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.ProdutoEspecial
{
    [DetalhePagina("\uE804", "Veículo")]
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

            BasicMainPage.Current.Retornar();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Retornar();
        }
    }
}
