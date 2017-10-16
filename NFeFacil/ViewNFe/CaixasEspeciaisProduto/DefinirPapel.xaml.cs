using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasEspeciaisProduto
{
    public sealed partial class DefinirPapel : ContentDialog
    {
        public DefinirPapel()
        {
            this.InitializeComponent();
        }

        public string NRECOPI { get; private set; }
    }
}
