using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class EscolherTipoICMS : ContentDialog
    {
        public EscolherTipoICMS()
        {
            this.InitializeComponent();
        }

        int regimeSelecionado;
        int RegimeSelecionado
        {
            get => regimeSelecionado;
            set
            {
                regimeSelecionado = value;
                cmbTipoICMSSN.IsEnabled = value == (int)Regimes.Simples;
                cmbTipoICMSRN.IsEnabled = value == (int)Regimes.Normal;
            }
        }

        public Regimes Regime => (Regimes)RegimeSelecionado;
        public int TipoICMSSN { get; private set; }
        public int TipoICMSRN { get; private set; }
        public int Origem { get; private set; }

        public enum Regimes
        {
            Simples,
            Normal
        }
    }
}
