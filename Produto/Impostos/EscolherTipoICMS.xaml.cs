using BaseGeral;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos
{
    public sealed partial class EscolherTipoICMS : ContentDialog
    {
        public EscolherTipoICMS()
        {
            InitializeComponent();
            var normal = DefinicoesTemporarias.EmitenteAtivo.RegimeTributario == 3;
            cmbTipoICMSRN.Visibility = normal ? Visibility.Visible : Visibility.Collapsed;
            cmbTipoICMSSN.Visibility = normal ? Visibility.Collapsed : Visibility.Visible;
        }

        public string TipoICMSSN { get; private set; }
        public string TipoICMSRN { get; private set; }
        public int Origem { get; private set; }
    }
}
