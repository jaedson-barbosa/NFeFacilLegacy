using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasEspeciaisProduto
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DefinirCombustivel : Page
    {
        public DefinirCombustivel()
        {
            this.InitializeComponent();
        }

        public Combustivel Comb { get; } = new Combustivel();

        bool UsarCIDE
        {
            get => grupoCide.Visibility == Visibility.Visible;
            set
            {
                Comb.CIDE = value ? new CIDE() : null;
                grupoCide.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
