using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFCe
{
    public sealed partial class DimensoesDANFE : ContentDialog
    {
        public double Largura { get; private set; } = 80;
        public double Margem { get; private set; } = 3;

        public DimensoesDANFE()
        {
            InitializeComponent();
        }
    }
}
