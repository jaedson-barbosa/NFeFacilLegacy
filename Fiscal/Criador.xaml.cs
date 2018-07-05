using BaseGeral;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    public sealed partial class Criador : ContentDialog
    {
        bool ambienteHomolocagao;
        bool AmbienteHomolocagao
        {
            get => ambienteHomolocagao;
            set
            {
                ambienteHomolocagao = value;
                if (DefinicoesPermanentes.CalcularNumeroNFe)
                {
                    CalcularNumero_Click(null, null);
                }
            }
        }

        ushort Serie { get; set; } = 1;

        IControleCriacao Controle { get; }

        public Criador(IControleCriacao controle)
        {
            Controle = controle;
            InitializeComponent();
            AmbienteHomolocagao = false;
        }

        void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var numero = (int)txtNumero.Number;
            Controle.Processar(Serie, numero, AmbienteHomolocagao);
        }

        void CalcularNumero_Click(object sender, RoutedEventArgs e)
        {
            txtNumero.Number = Controle.ObterMaiorNumero(Serie, AmbienteHomolocagao) + 1;
        }
    }
}
