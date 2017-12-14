using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class EscolherTipoEndereco : ContentDialog
    {
        public int TipoEscolhido { get; private set; }
        public bool Nacional => TipoEscolhido == 0;

        public EscolherTipoEndereco()
        {
            InitializeComponent();
        }
    }
}
