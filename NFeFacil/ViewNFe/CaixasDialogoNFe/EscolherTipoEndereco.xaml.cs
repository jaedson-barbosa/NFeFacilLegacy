using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class EscolherTipoEndereco : ContentDialog
    {
        public TipoEndereco TipoEscolhido { get; private set; }

        public EscolherTipoEndereco()
        {
            InitializeComponent();
            cmbTipo.ItemsSource = ExtensoesPrincipal.ObterItens<TipoEndereco>();
        }
    }

    public enum TipoEndereco
    {
        Nacional,
        Exterior
    }
}
