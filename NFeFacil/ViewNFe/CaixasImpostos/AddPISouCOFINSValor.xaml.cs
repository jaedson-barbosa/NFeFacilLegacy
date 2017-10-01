using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AddPISouCOFINSValor : ContentDialog
    {
        public AddPISouCOFINSValor()
        {
            this.InitializeComponent();
        }

        public double Valor { get; private set; }
    }
}
