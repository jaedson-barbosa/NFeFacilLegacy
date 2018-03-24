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
    [DetalhePagina("\uEB42", "Combustivel")]
    public sealed partial class DefinirCombustivel : Page
    {
        Combustivel Comb { get; set; }

        public DefinirCombustivel()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var prod = e.Parameter as IProdutoEspecial;
            Comb = prod?.comb ?? new Combustivel();
        }

        bool UsarCIDE
        {
            get => grupoCide.Visibility == Visibility.Visible;
            set
            {
                Comb.CIDE = value ? new CIDE() : null;
                grupoCide.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Concluido(object sender, RoutedEventArgs e)
        {
            var ultFrame = Frame.BackStack[Frame.BackStack.Count - 1];
            var prod = (IProdutoEspecial)ultFrame.Parameter;
            prod.veicProd = null;
            prod.medicamentos = null;
            prod.armas = null;
            prod.comb = Comb;
            prod.NRECOPI = null;

            BasicMainPage.Current.Retornar();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            BasicMainPage.Current.Retornar();
        }
    }
}
