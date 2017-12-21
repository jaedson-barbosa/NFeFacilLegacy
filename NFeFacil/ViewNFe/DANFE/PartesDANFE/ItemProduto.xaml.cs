using NFeFacil.ViewNFe.DANFE.Pacotes;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.ViewNFe.DANFE.PartesDANFE
{
    public sealed partial class ItemProduto : UserControl
    {
        DimensoesCampoProdutos DimensoesLocal { get; } = new DimensoesCampoProdutos();

        DadosProduto contexto;
        public DadosProduto Contexto
        {
            get => contexto;
            set
            {
                contexto = value;
                InitializeComponent();
            }
        }
    }
}
