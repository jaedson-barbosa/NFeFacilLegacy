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
    public sealed partial class DefinirCombustivel : Page
    {
        Combustivel Comb { get; } = new Combustivel();

        public DefinirCombustivel()
        {
            InitializeComponent();
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
            var prod = ((DetalhesProdutos)ultFrame.Parameter).Produto;
            prod.veicProd = null;
            prod.medicamentos = null;
            prod.armas = null;
            prod.comb = Comb;
            prod.NRECOPI = null;

            MainPage.Current.Retornar();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
