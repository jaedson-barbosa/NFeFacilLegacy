using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class ItemProduto : UserControl
    {
        DimensoesCampoProdutos DimensoesLocal { get; } = new DimensoesCampoProdutos();

        public ItemProduto()
        {
            InitializeComponent();
        }
    }
}
